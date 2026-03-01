using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using Unity.Collections;
using UnityEngine.Experimental.Rendering;

public class Photography : MonoBehaviour
{
    public Camera photoCamera;
    public int photoWidth = 1920;
    public int photoHeight = 1080;

    public InputAction takePhoto;

    private void Update()
    {
        if (takePhoto.triggered)
        {
            TakePhoto();
        }
    }

    public void TakePhoto()
    {
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

            // Clean up the RenderTexture once we have the data
            rt.Release();
            Destroy(rt);
        });
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

        Debug.Log($"Photo saved to: {path}");

        // Update the gallery incrementally on the main thread
        var gallery = FindFirstObjectByType<GalleryDisplay>();
        if (gallery != null)
        {
            gallery.AddPhoto(path);
        }
    }
}
