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
        if (Speedlevel_1 != null) Speedlevel_1.enabled = false;
        if (Speedlevel_2 != null) Speedlevel_2.enabled = false;
        if (Speedlevel_3 != null) Speedlevel_3.enabled = false;
    }

    private void Update()
    {
        
        if (speedometerScript != null)
        {
            float currentSpeed = speedometerScript.maxSpeed;


            if (currentSpeed > 15f)
            {
                if (Speedlevel_1 != null) Speedlevel_1.gameObject.SetActive(true);
            }
            if (currentSpeed < 15f)
            {
                if (Speedlevel_1 != null) Speedlevel_1.gameObject.SetActive(false);
            }
            if (currentSpeed > 40f && currentSpeed <= 130f)
            {
                if (Speedlevel_2 != null) Speedlevel_2.gameObject.SetActive(true);
            }
            if (currentSpeed < 40f)
            {
                if (Speedlevel_2 != null) Speedlevel_3.gameObject.SetActive(true);

            }
            if (currentSpeed > 80f)
            {
                if (Speedlevel_3 != null) Speedlevel_3.gameObject.SetActive(true);
            }
            if (currentSpeed < 80f)
            {
                if (Speedlevel_3 != null) Speedlevel_3.gameObject.SetActive(false);
            }
        }


    }

}
