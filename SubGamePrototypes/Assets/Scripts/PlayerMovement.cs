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
                interactionText.text = "Press E to control Hercules";
                interactionUI.SetActive(true);
                //Debug.Log("Hit herc controls");
                if(Input.GetKey(KeyCode.E))
                {
                    inputManager.state = InputManager.InputState.Hercules;
                    interactionUI.SetActive(false);
                }
            }
            else if (hit.collider.CompareTag("Journal"))
            {
                interactionText.text = "Press E to open Journal";
                interactionUI.SetActive(true);
                //Debug.Log("Hit herc controls");
                if (interact.IsPressed())
                {
                    inputManager.state = InputManager.InputState.Menus;
                    uiManager.journalCanvas.enabled = true;
                    uiManager.activeCanvas = uiManager.journalCanvas;
                    interactionUI.SetActive(false);
                }
            }
            else
            {
                interactionUI.SetActive(false);
            }
        }
        else
        {
            interactionUI.SetActive(false);
        }

    }
}
