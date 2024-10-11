using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class BasicFpMovement : MonoBehaviour
{
    private static BasicFpMovement Instance;

    // Public variables for crouching
    public float crouchHeight = 0.5f;
    public float standingHeight = 1.0f;
    public float crouchSpeed = 0.5f;

    // Movement variables
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    // Ceiling and crouch settings
    public float lowCeilingRange = 2.0f;
    public LayerMask ceilingLayer;

    // Private variables
    private CharacterController controller;
    private Vector3 velocity;

    // Input action asset reference
    private PlayerInputAsset inputActions; // Input Action Asset class

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction crouchAction;

    void Awake()
    {
        // Ensure only one instance exists (singleton)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject); // Destroy duplicate instances
            return;
        }

        // Initialize Input Action Asset if it's null
        if (inputActions == null)
        {
            inputActions = new PlayerInputAsset();
        }
    }

    void OnEnable()
    {
        // Ensure the input actions are correctly initialized
        if (inputActions == null)
        {
            inputActions = new PlayerInputAsset(); // Reinitialize if null
        }

        // Enable the input actions
        moveAction = inputActions.PlayerInput.Movement;
        jumpAction = inputActions.PlayerInput.Jump;
        crouchAction = inputActions.PlayerInput.Crouch;

        moveAction.Enable();
        jumpAction.Enable();
        crouchAction.Enable();
    }

    void OnDisable()
    {
        // Disable the input actions to prevent conflicts
        if (moveAction != null) moveAction.Disable();
        if (jumpAction != null) jumpAction.Disable();
        if (crouchAction != null) crouchAction.Disable();
    }

    void Start()
    {
        // Initialize CharacterController
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        if(inputActions == null)
        {
            inputActions = new PlayerInputAsset();
        }

        Vector3 currentScale = transform.localScale;

        // Crouch logic
        if (crouchAction.IsPressed())
        {
            currentScale.y = Mathf.Max(currentScale.y - (Time.deltaTime * crouchSpeed), crouchHeight);
        }
        else
        {
            RaycastHit hitCeiling;
            if (!Physics.Raycast(transform.position, Vector3.up, out hitCeiling, lowCeilingRange, ceilingLayer))
            {
                currentScale.y = Mathf.Min(currentScale.y + (Time.deltaTime * crouchSpeed), standingHeight);
            }
        }

        transform.localScale = currentScale;

        // Movement input (using Vector2 from Input System)
        Vector2 input = moveAction.ReadValue<Vector2>();
        float x = input.x;
        float z = input.y;

        // Move relative to the player's orientation
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensures the player stays grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Jump logic
        if (jumpAction.triggered && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}