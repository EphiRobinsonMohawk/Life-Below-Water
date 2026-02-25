using System.Collections.Generic;
using UnityEngine;

public class HandCollisionDetector : MonoBehaviour
{
    // Keep internal list private to avoid external modification
    private List<Rigidbody> _collidingBodies = new List<Rigidbody>();

    // Map internal list to public read-only property, but clean up nulls/inactive objects first
    public List<Rigidbody> CollidingBodies
    {
        get
        {
            _collidingBodies.RemoveAll(rb => rb == null || !rb.gameObject.activeInHierarchy);
            return _collidingBodies;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null && rb.CompareTag("Grabbable") && !_collidingBodies.Contains(rb))
        {
            _collidingBodies.Add(rb);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null && _collidingBodies.Contains(rb))
        {
            _collidingBodies.Remove(rb);
        }
    }
}
