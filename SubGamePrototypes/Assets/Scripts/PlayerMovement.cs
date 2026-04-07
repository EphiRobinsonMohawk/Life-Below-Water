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

    // Things to look at
    public Transform HerculesScreen;
    public Transform Journal;
    public Transform ExpeditionTimer;
    public Transform TaskBoard;

    // Interaction Popup
    public GameObject interactionUI;
    public TMP_Text interactionText;

    public InputManager inputManager;
    public UIManager uiManager;
    public InputAction look;
    public InputAction interact;


    // Private variables
    private Transform targetTransform;
    private Vector2 lookPosition;
    private Transform[] targets;
    private int currentTargetIndex = 1; // Start at HerculesScreen
    private bool lookInputReset = true;
    private bool isLookingUp = false;
    public float flickThreshold = 0.5f;

    public void Start()
    {
        look = InputSystem.actions.FindAction("Player/Look");
        interact = InputSystem.actions.FindAction("Player/Interact");

        // Initialization
        targets = new Transform[] { Journal, HerculesScreen, TaskBoard };
    }

    // Always rotate
    public void FixedUpdate()
    {
        if (inputManager.state == InputManager.InputState.ControlRoom)
        {
            // Set current target based on index/vertical state
            if (targets != null && targets.Length > 0)
            {
                if (isLookingUp)
                {
                    targetTransform = ExpeditionTimer;
                }
                else
                {
                    targetTransform = targets[currentTargetIndex];
                }
                HandleLookAtTarget();
            }
        }
        else if (targetTransform != null)
        {

            HandleLookAtTarget();
        }
    }

    public void PlayerRotation()
    {
        Vector2 input = look.ReadValue<Vector2>();

        // Handle cycling targets with flicks
        if (lookInputReset)
        {
            // Vertical flicks (only from Hercules Screen)
            if (input.y > flickThreshold && currentTargetIndex == 1 && !isLookingUp)
            {
                isLookingUp = true;
                lookInputReset = false;
                return;
            }
            else if (input.y < -flickThreshold && isLookingUp)
            {
                isLookingUp = false;
                lookInputReset = false;
                return;
            }

            // Horizontal flicks (only while not looking up)
            if (!isLookingUp)
            {
                if (input.x > flickThreshold)
                {
                    // Flick Right -> Next target
                    if (currentTargetIndex < targets.Length - 1)
                    {
                        currentTargetIndex++;
                        lookInputReset = false;
                        return;
                    }
                }
                else if (input.x < -flickThreshold)
                {
                    // Flick Left -> Previous target
                    if (currentTargetIndex > 0)
                    {
                        currentTargetIndex--;
                        lookInputReset = false;
                        return;
                    }
                }
            }
        }
        else
        {
            // Reset look if stick returns near center or mouse stops
            if (Mathf.Abs(input.x) < flickThreshold * 0.5f && Mathf.Abs(input.y) < flickThreshold * 0.5f)
            {
                lookInputReset = true;
            }
        }
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
