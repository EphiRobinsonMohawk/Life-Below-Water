using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Camera cam;
    public float mouseSens = 2f;
    float cameraVertRot;
    public Transform player;
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
        float x = mouse.x * mouseSens;
        float y = mouse.y * mouseSens;
        cameraVertRot -= y;
        cameraVertRot = Mathf.Clamp(cameraVertRot, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVertRot;
        player.Rotate(Vector3.up * x);
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
                    uiManager.OpenJournal();
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
        Debug.Log("Hiding interaction UI");
        interactionUI.SetActive(false);
    }
}
