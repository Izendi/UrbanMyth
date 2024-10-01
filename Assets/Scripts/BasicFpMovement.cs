using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BasicFpMovement : MonoBehaviour
{
    public KeyCode CrouchKey = KeyCode.C;
    public float CrouchHeight = 0.5f;
    public float StandingHeight = 1.0f;
    public float CrouchSpeed = 0.5f;

    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    public float lowCeilingRange = 2.0f;
    public LayerMask ceilingLayer;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 currentScale = transform.localScale;

        if (Input.GetKey(CrouchKey))
        {
 
            currentScale.y = Mathf.Max(currentScale.y - (Time.deltaTime * CrouchSpeed), CrouchHeight);

        }
        else
        {

            RaycastHit hitCeiling;

            if (Physics.Raycast(transform.position, Vector3.up, out hitCeiling, lowCeilingRange, ceilingLayer))
            {

            }
            else
            {
                currentScale.y = Mathf.Min(currentScale.y + (Time.deltaTime * CrouchSpeed), StandingHeight);
            }

            
        }

        transform.localScale = currentScale;

        // Get input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move relative to the player's orientation
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensures the player stays grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        // Jumping
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensures the player stays grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}