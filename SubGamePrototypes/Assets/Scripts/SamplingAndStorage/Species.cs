using UnityEngine;

public enum SpeciesType
{
    Fish,
    Plant,
    Invertebrate,
}

public class Species : MonoBehaviour
{
    public SpeciesType Type = SpeciesType.Plant;
    public bool isSampleable = true;
    public bool hasBeenRecorded => JournalManager.Instance != null && JournalManager.Instance.IsSpeciesIdentified(this);
    public float maximumPhotoDistance = 10f;

    public string speciesName = "";

    [Header("Photography Settings")]
    public Vector3 photoBoundsCenter;
    public Vector3 photoBoundsSize = Vector3.one;

    public Bounds GetWorldBounds()
    {
        // Construct local bounds
        Bounds localBounds = new Bounds(photoBoundsCenter, photoBoundsSize);
        
        // Transform local bounds to world space AABB
        Vector3 center = transform.TransformPoint(localBounds.center);
        Vector3 extents = localBounds.extents;
        
        // We need to calculate the world-space AABB that encapsulates the rotated local box
        Vector3 axisX = transform.TransformVector(new Vector3(extents.x, 0, 0));
        Vector3 axisY = transform.TransformVector(new Vector3(0, extents.y, 0));
        Vector3 axisZ = transform.TransformVector(new Vector3(0, 0, extents.z));

        Vector3 newExtents = new Vector3(
            Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x),
            Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y),
            Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z)
        );

        return new Bounds(center, newExtents * 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(photoBoundsCenter, photoBoundsSize);
    }

    private void Start()
    {
        if (isSampleable)
        {
            // Add a sample component to this species so it can be collected
            Sample sample = gameObject.AddComponent<Sample>();
            sample.Type = SampleType.Species;
        }
    }
}
