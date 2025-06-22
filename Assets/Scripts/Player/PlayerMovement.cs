using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // components
    CharacterController controller;
    [SerializeField]
    Transform cameraTransform;
    Player player;

    // (to be tweaked externally) variables
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float lookSensitivity = 5f;
    [SerializeField]
    float sprintModifier = 2f;
    [SerializeField]
    float jumpVelocity = 3;

    // input actions
    InputAction moveAction;
    InputAction lookAction;
    InputAction sprintAction;
    InputAction crouchAction;
    InputAction jumpAction;

    private bool canMove = true;
    Vector3 velocity = Vector3.zero;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        jumpAction = InputSystem.actions.FindAction("Jump");

        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();

        jumpAction.started += Jump;
    }

    void Update()
    {
        if (!canMove) return;

        if (!player.IsGrounded()) velocity.y -= 9.81f * Time.deltaTime;
        else if(controller.velocity.y == 0 && velocity.y < 0) velocity.y = 0;

        // handle movement
        Vector2 direction2D = moveAction.ReadValue<Vector2>();
        direction2D = direction2D.normalized; // so the player doesn't move faster walking diagonal
        direction2D *= speed;
        direction2D *= Time.deltaTime; // so the player moves at same speed regardless of FPS

        // check if player is sprinting and modify speed accordingly
        direction2D *= sprintAction.IsPressed() ? sprintModifier : 1;

        Vector3 direction = direction2D.x * transform.right + direction2D.y * transform.forward; // convert 2d direction to 3d
        controller.Move(direction + velocity * Time.deltaTime);

        // TODO: add crouching

        // handle look
        Vector2 look2D = lookAction.ReadValue<Vector2>();
        look2D *= lookSensitivity;
        look2D *= Time.deltaTime; // so the camera doesn't move extra with high FPS

        // TODO: clamp player look

        transform.Rotate(Vector3.up * look2D.x); // rotate player horizontally so movement correlates with look
        cameraTransform.Rotate(Vector3.right * -look2D.y);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        if (player.IsGrounded())
            velocity.y = jumpVelocity;
    }
}
