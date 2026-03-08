using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using Unity.Collections;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Events;
using System.Collections.Generic;

public class Photography : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<Dictionary<Species, bool>> onSpeciesIdentified = new UnityEvent<Dictionary<Species, bool>>();

    public Camera photoCamera;
    public int photoWidth = 1920;
    public int photoHeight = 1080;

    public InputActionReference takePhoto;
    public ShutterEffect shutterEffect;

    private void Update()
    {
        if (takePhoto.action.triggered)
        {
            TakePhoto();
        }
    }

    public void TakePhoto()
    {
        if (shutterEffect != null)
        {
            shutterEffect.TriggerEffect();
        }

        // 1. Create a temporary Render Texture
        RenderTexture rt = new RenderTexture(photoWidth, photoHeight, 24);
        photoCamera.targetTexture = rt;

        // 2. Manually tell the camera to render its view to that texture
        photoCamera.Render();

        // IMMEDIATELY restore the camera's target to the screen (null)
        // This prevents the "No Cameras Rendering" message while waiting for the GPU readback
        photoCamera.targetTexture = null;

        // 3. Request an asynchronous readback from the GPU
        AsyncGPUReadback.Request(rt, 0, request => {
            if (request.hasError) {
                Debug.LogError("GPU readback error occurred.");
                return;
            }

            // Start the background saving process
            SaveImageAsync(request.GetData<byte>(), photoWidth, photoHeight);

            // Identify species in frame
            IdentifySpeciesInFrame();

            // Clean up the RenderTexture once we have the data
            rt.Release();
            Destroy(rt);
        });
    }

    private void IdentifySpeciesInFrame()
    {
        Species[] allSpecies = Object.FindObjectsByType<Species>(FindObjectsSortMode.None);
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(photoCamera);
        Dictionary<Species, bool> identifiedSpecies = new Dictionary<Species, bool>();

        foreach (Species species in allSpecies)
        {
            // 1. Initial rough pre-filter using AABB distance
            Bounds worldBounds = species.GetWorldBounds();
            float maxDim = Mathf.Max(species.photoBoundsSize.x, species.photoBoundsSize.y, species.photoBoundsSize.z);
            float effectiveMaxDistance = species.maximumPhotoDistance + (2f * maxDim);
            float sqrEffectiveDistance = effectiveMaxDistance * effectiveMaxDistance;

            float sqrDistancePre = worldBounds.SqrDistance(photoCamera.transform.position);
            if (sqrDistancePre > sqrEffectiveDistance) continue;

            // 2. Frustum check using bounds
            if (!GeometryUtility.TestPlanesAABB(frustumPlanes, worldBounds)) continue;

            // 3. Refined distance check (to closest point on bounds)
            float sqrDistance = worldBounds.SqrDistance(photoCamera.transform.position);
            if (sqrDistance > sqrEffectiveDistance) continue;

            // 4. Multi-point visibility check (occlusion)
            if (IsSpeciesVisible(species, worldBounds))
            {
                identifiedSpecies.Add(species, species.hasBeenRecorded);
                if (!species.hasBeenRecorded)
                {
                    JournalManager.Instance.RecordSpecies(species);
                    //Debug.Log($"Identified new species: {species.gameObject.name}");
                }
                else
                {
                    //Debug.Log($"Photographed known species: {species.gameObject.name}");
                }
            }
        }

        if (identifiedSpecies.Count > 0)
        {
            onSpeciesIdentified.Invoke(identifiedSpecies);
        }
    }

    private bool IsSpeciesVisible(Species species, Bounds worldBounds)
    {
        // Check center and corners of the manual bounding box defined in local space
        Vector3 localCenter = species.photoBoundsCenter;
        Vector3 localExtents = species.photoBoundsSize * 0.5f;

        Vector3[] localCheckPoints = new Vector3[]
        {
            Vector3.zero, // Pivot (Origin)
            localCenter,  // Manual bounds center
            localCenter + new Vector3(localExtents.x, localExtents.y, localExtents.z),
            localCenter + new Vector3(localExtents.x, localExtents.y, -localExtents.z),
            localCenter + new Vector3(localExtents.x, -localExtents.y, localExtents.z),
            localCenter + new Vector3(localExtents.x, -localExtents.y, -localExtents.z),
            localCenter + new Vector3(-localExtents.x, localExtents.y, localExtents.z),
            localCenter + new Vector3(-localExtents.x, localExtents.y, -localExtents.z),
            localCenter + new Vector3(-localExtents.x, -localExtents.y, localExtents.z),
            localCenter + new Vector3(-localExtents.x, -localExtents.y, -localExtents.z)
        };

        foreach (Vector3 localPoint in localCheckPoints)
        {
            Vector3 worldPoint = species.transform.TransformPoint(localPoint);

            // First, ensure this point is actually in the viewport
            Vector3 viewportPos = photoCamera.WorldToViewportPoint(worldPoint);
            bool inViewport = viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0;
            
            if (!inViewport) continue;

            // Linecast to check for occlusion
            if (Physics.Linecast(photoCamera.transform.position, worldPoint, out RaycastHit hit))
            {
                // If we hit the species or any of its children, it's visible
                if (hit.collider.gameObject == species.gameObject || hit.collider.transform.IsChildOf(species.transform))
                {
                    return true;
                }
            }
            else
            {
                // If it hit nothing, it's definitely visible
                return true;
            }
        }

        return false;
    }

    private async void SaveImageAsync(NativeArray<byte> rawData, int width, int height)
    {
        string filename = $"Photo_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string path = Path.Combine(Application.persistentDataPath, filename);

        // We need to copy the native array because the original will be disposed after the callback
        NativeArray<byte> dataCopy = new NativeArray<byte>(rawData, Allocator.Persistent);

        // Offload PNG encoding and disk IO to a background thread
        await Task.Run(() =>
        {
            try
            {
                // Encode the raw bytes from the GPU to PNG
                // Note: ImageConversion.EncodeNativeArrayToPNG often returns NativeArray<byte> in newer Unity versions
                var pngBytes = ImageConversion.EncodeNativeArrayToPNG(dataCopy, GraphicsFormat.R8G8B8A8_SRGB, (uint)width, (uint)height);
                
                try
                {
                    // File.WriteAllBytes expects byte[]. NativeArray can be converted with ToArray().
                    File.WriteAllBytes(path, pngBytes.ToArray());
                }
                finally
                {
                    // For modern Unity, pngBytes is a NativeArray and must be disposed.
                    // If it were a byte[], ToArray() would still work (Linq) but disposal would need a check.
                    if (pngBytes is System.IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
            finally
            {
                // Always dispose of persistent native arrays
                dataCopy.Dispose();
            }
        });

        //Debug.Log($"Photo saved to: {path}");

        // Update the gallery incrementally on the main thread
        var gallery = FindFirstObjectByType<GalleryDisplay>();
        if (gallery != null)
        {
            gallery.AddPhoto(path);
        }
    }
}
