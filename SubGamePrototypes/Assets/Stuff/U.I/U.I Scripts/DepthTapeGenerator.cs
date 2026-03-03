using UnityEngine;
using TMPro;

public class DepthTapeGenerator : MonoBehaviour
{
    public RectTransform depthContent;
    public TextMeshProUGUI numberTemplate;

    public int maxDepth = 500;
    public int step = 10;
    public float pixelsPerMeter = 4f;

    void Start()
    {
        GenerateNumbers();
    }

    void GenerateNumbers()
    {
        float totalHeight = maxDepth * pixelsPerMeter;
        float halfHeight = totalHeight / 2f;

        for (int depth = 0; depth <= maxDepth; depth += step)
        {
            TextMeshProUGUI number = Instantiate(numberTemplate, depthContent);
            number.text = depth.ToString();

            RectTransform rt = number.GetComponent<RectTransform>();

            float yPos = (depth * pixelsPerMeter) - halfHeight;
            rt.anchoredPosition = new Vector2(0, yPos);
        }

        numberTemplate.gameObject.SetActive(false);
    }
}