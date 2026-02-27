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
        for (int depth = 0; depth <= maxDepth; depth += step)
        {
            TextMeshProUGUI newNumber = Instantiate(numberTemplate, depthContent);
            newNumber.text = depth.ToString();

            RectTransform rt = newNumber.GetComponent<RectTransform>();

            float yPos = depth * pixelsPerMeter;
            rt.anchoredPosition = new Vector2(0, yPos);
        }

        numberTemplate.gameObject.SetActive(false);
    }
}