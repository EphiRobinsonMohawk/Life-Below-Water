using UnityEngine;

public class rotate : MonoBehaviour
{
    public GameObject clam;
    public float rotationSpeed = 30f; // degrees per second
    public bool rotatesForwards;
    void Update()
    {
        if (clam != null)
        {
            if (rotatesForwards) clam.transform.Rotate(0f, rotationSpeed, (rotationSpeed * 0.5f) * Time.deltaTime);
            if (!rotatesForwards) clam.transform.Rotate(0f, -rotationSpeed, -(rotationSpeed * 0.5f) * Time.deltaTime);
        }
    }
}
