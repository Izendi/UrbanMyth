using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalStateManager : MonoBehaviour
{
    public static GlobalStateManager Instance { get; private set; }

    // Input action asset reference
    public static PlayerInputAsset inputActions; // Input Action Asset class

    private PlayerInput _playerInput;

    //Ref to player object
    public GameObject Player;

    // Reference to the MenuOpen action
    private InputAction menuOpenAction;

    public bool isMenuOpenPressed = false;
    public bool isPlayerDead = false;

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

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction crouchAction;


    private void Awake()
    {
        // Ensure that there's only one instance of the GlobalStateManager
        if (Instance == null)
        {
            Instance = this;

            

            DontDestroyOnLoad(gameObject);  // Keep it alive between scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }

        _playerInput = GetComponent<PlayerInput>();

        inputActions = new PlayerInputAsset();
        menuOpenAction = inputActions.PlayerInput.MenuOpen;

    }

    private void OnEnable()
    {
        // Enable the input actions
        moveAction = inputActions.PlayerInput.Movement;
        jumpAction = inputActions.PlayerInput.Jump;
        crouchAction = inputActions.PlayerInput.Crouch;

        moveAction.Enable();
        jumpAction.Enable();
        crouchAction.Enable();

        menuOpenAction = inputActions.PlayerInput.MenuOpen;

        menuOpenAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the action
        menuOpenAction.Disable();

        // Disable the input actions
        moveAction.Disable();
        jumpAction.Disable();
        crouchAction.Disable();
    }

    void Start()
    {
        // Initialize CharacterController
        controller = Player.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (menuOpenAction.WasPerformedThisFrame())
        {
            Debug.Log("Menu Button was pressed key was pressed!");
        }

        Vector3 currentScale = Player.transform.localScale;

        // Crouch logic
        if (crouchAction.IsPressed())
        {
            currentScale.y = Mathf.Max(currentScale.y - (Time.deltaTime * crouchSpeed), crouchHeight);
        }
        else
        {
            RaycastHit hitCeiling;
            if (!Physics.Raycast(Player.transform.position, Vector3.up, out hitCeiling, lowCeilingRange, ceilingLayer))
            {
                currentScale.y = Mathf.Min(currentScale.y + (Time.deltaTime * crouchSpeed), standingHeight);
            }
        }

        Player.transform.localScale = currentScale;

        // Movement input (using Vector2 from Input System)
        Vector2 input = moveAction.ReadValue<Vector2>();
        float x = input.x;
        float z = input.y;

        // Move relative to the player's orientation
        Vector3 move = Player.transform.right * x + Player.transform.forward * z;
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
