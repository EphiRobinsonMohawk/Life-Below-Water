using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Camera cam;
    public float mouseSens = 2f;
    float cameraVertRot;
    public Transform player;
    public float rotationSmoothness = 10f; // Lower = more "weight", Higher = snappier
    Vector2 currentRotation;

    // Interaction Popup
    public GameObject interactionUI;
    public TMP_Text interactionText;

    public InputManager inputManager;
    public UIManager uiManager;
    public InputAction look;
    public InputAction interact;

    public void Start()
    {
        look = InputSystem.actions.FindAction("Player/Look");
        interact = InputSystem.actions.FindAction("Player/Interact");
    }

    public void PlayerRotation()
    {
        Vector2 mouse = look.ReadValue<Vector2>();
        
        // Target rotation based on input
        float targetX = mouse.x * mouseSens;
        float targetY = mouse.y * mouseSens;

        // Smoothly interpolate towards target rotation
        currentRotation.x = Mathf.Lerp(currentRotation.x, targetX, Time.deltaTime * rotationSmoothness);
        currentRotation.y = Mathf.Lerp(currentRotation.y, targetY, Time.deltaTime * rotationSmoothness);

        cameraVertRot -= currentRotation.y;
        cameraVertRot = Mathf.Clamp(cameraVertRot, -90f, 90f);
        
        transform.localEulerAngles = Vector3.right * cameraVertRot;
        player.Rotate(Vector3.up * currentRotation.x);
    }

    public void Interaction()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 10, Color.goldenRod);
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 25f))
        {
            if(hit.collider.CompareTag("HerculesControls"))
            {
                interactionText.text = "Press A to control Hercules";
                interactionUI.SetActive(true);
                //Debug.Log("Hit herc controls");
                if(interact.IsPressed())
                {
                    inputManager.state = InputManager.InputState.Hercules;
                    interactionText.text = "Press Select for a First Person View";
                }
            }
            else if (hit.collider.CompareTag("Journal"))
            {
                interactionText.text = "Press A to open Journal";
                interactionUI.SetActive(true);
                //Debug.Log("Hit herc controls");
                if (interact.IsPressed())
                {
                    inputManager.state = InputManager.InputState.Menus;
                    uiManager.OpenExpeditions();
                    HideInteractionUI();
                }
            }
            else
            {
                HideInteractionUI();
            }
        }
        else
        {
            HideInteractionUI();
        }

    }

    public void ShowExitROV()
    {
        interactionText.text = "Press Start to exit Hercules";
        interactionUI.SetActive(true);
    }

    public void HideInteractionUI()
    {
        //Debug.Log("Hiding interaction UI");
        interactionUI.SetActive(false);
    }
}
