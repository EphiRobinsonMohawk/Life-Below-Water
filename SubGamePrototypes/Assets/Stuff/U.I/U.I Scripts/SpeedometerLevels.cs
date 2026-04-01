using UnityEngine;
using UnityEngine.UI;

public class SpeedometerLevels : MonoBehaviour
{

    [SerializeField] private Speedometer speedometerScript;

    [SerializeField] private Image Speedlevel_1;
    [SerializeField] private Image Speedlevel_2;
    [SerializeField] private Image Speedlevel_3;



    private void Start()
    {
        Speedlevel_1.enabled = false;
        Speedlevel_2.enabled = false;
        Speedlevel_3.enabled = false;
    }

    private void Update()
    {
        float currentSpeed = speedometerScript.speed;

        if (currentSpeed > 15f)
        {
            Speedlevel_1.enabled = true;
        }
        if (currentSpeed < 15f)
        {
            Speedlevel_1.enabled = false;
        }
        if (currentSpeed > 35f)
        {
            Speedlevel_2.enabled = true;
        }
        if (currentSpeed < 35f)
        {
            Speedlevel_2.enabled = false;

        }
        if (currentSpeed > 60f)
        {
            Speedlevel_3.enabled = true;
        }
        if (currentSpeed < 60f)
        {
            Speedlevel_3.enabled = false;
        }
    }

}
