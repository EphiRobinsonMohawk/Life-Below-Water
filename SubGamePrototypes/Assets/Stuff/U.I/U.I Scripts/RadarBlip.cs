using UnityEngine;
using UnityEngine.UI;

public class RadarBlip : MonoBehaviour
{
    public float lifeTime = 1.5f;

    private Image image;
    private float timer;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / lifeTime;

        float alpha = Mathf.Lerp(1f, 0f, t);
        float scale = Mathf.Lerp(1f, 1.5f, t);

        Color c = image.color;
        c.a = alpha;
        image.color = c;

        transform.localScale = Vector3.one * scale;

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}