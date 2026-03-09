using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Camera cam;
    public float mouseSens = 2f;
    float cameraVertRot;
    public Transform player;
    public float rotationSmoothness = 7f; // Lower = more "weight"
    public float autoLookSlow = 0.3f; // How much to slow down auto-look
    Vector2 currentRotation;
    public Transform HerculesScreen;
    public Transform Journal;
    private Transform targetTransform;

    // Interaction Popup
    public GameObject interactionUI;
    public TMP_Text interactionText;

    public InputManager inputManager;
    public UIManager uiManager;
    public InputAction look;
    public InputAction interact;

    // Private variables
    Vector2 lookPosition;

    public void Start()
    {
        look = InputSystem.actions.FindAction("Player/Look");
        interact = InputSystem.actions.FindAction("Player/Interact");
    }

    // Always rotate
    public void FixedUpdate()
    {
        if (inputManager.state == InputManager.InputState.ControlRoom)
        {
            targetTransform = null;
            // Smoothly interpolate towards target rotation
            currentRotation.x = Mathf.LerpAngle(currentRotation.x, lookPosition.x, Time.deltaTime * rotationSmoothness);
            currentRotation.y = Mathf.LerpAngle(currentRotation.y, lookPosition.y, Time.deltaTime * rotationSmoothness);

            cameraVertRot -= currentRotation.y;
            cameraVertRot = Mathf.Clamp(cameraVertRot, -90f, 90f);

            transform.localEulerAngles = Vector3.right * cameraVertRot;
            player.Rotate(Vector3.up * currentRotation.x);
        }
        else if (targetTransform != null)
        {

            HandleLookAtTarget();
        }
    }

    public void PlayerRotation()
    {
        lookPosition = look.ReadValue<Vector2>();
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
                    LookAtHercules();
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

    public void LookAtHercules()
    {
        targetTransform = HerculesScreen;
    }

    public void LookAtJournal()
    {
        targetTransform = Journal;
    }

    private void HandleLookAtTarget()
    {
        if (targetTransform == null) return;

        // Direction to target
        Vector3 direction = (targetTransform.position - cam.transform.position).normalized;

        // Horizontal rotation (Player body)
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * rotationSmoothness * autoLookSlow);

        // Vertical rotation (Camera)
        // Calculate the angle between the horizontal plane and the direction to target
        float targetVertRot = -Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        cameraVertRot = Mathf.LerpAngle(cameraVertRot, targetVertRot, Time.deltaTime * rotationSmoothness * autoLookSlow);

        cameraVertRot = Mathf.Clamp(cameraVertRot, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVertRot;
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
