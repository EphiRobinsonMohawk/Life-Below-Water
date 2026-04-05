using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SubCollision : MonoBehaviour
{
    public ScoreManager scoreManager;
    bool collisionCooldown;
    public SubMovement sub;

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionCooldown) return;
        if (sub.rb.linearVelocity.sqrMagnitude < 1) return;
        Debug.Log("Collidied with " + collision.gameObject + " and lost funds.");
        scoreManager.ChangeFunds(-150, "Crashed ROV!");
        collisionCooldown = true;
        StartCoroutine(EnableCollision());
    }

    IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(1.5f);
        collisionCooldown = false;
        Debug.Log("Collision Reenabled");
    }
}
