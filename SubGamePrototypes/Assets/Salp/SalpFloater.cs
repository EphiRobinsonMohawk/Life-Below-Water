using System.Collections.Generic;
using UnityEngine;

public class SalpFloater : MonoBehaviour
{
    public List<GameObject> salps = new List<GameObject>();

    private Dictionary<GameObject, float> startY = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, int> direction = new Dictionary<GameObject, int>();

    public float floatHeight = 3f;
    public float speed = 0.2f;

    public void AddSalp(GameObject salp)
    {
        salps.Add(salp);

        startY[salp] = salp.transform.position.y;
        direction[salp] = 1; // 1 = going up, -1 = going down
    }

    private void Update()
    {
        foreach (GameObject salp in salps)
        {
            float y = salp.transform.position.y;

            float top = startY[salp] + floatHeight;
            float bottom = startY[salp] - floatHeight;

            y += direction[salp] * speed * Time.deltaTime;

            if (y >= top) direction[salp] = -1;
            if (y <= bottom) direction[salp] = 1;

            salp.transform.position = new Vector3(
                salp.transform.position.x,
                y,
                salp.transform.position.z
            );
        }
    }
}
