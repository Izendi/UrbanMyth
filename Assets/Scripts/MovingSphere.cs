using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    [SerializeField, Range(0.0f, 100.0f)]
    float maxSpeed = 10.0f;

    [SerializeField, Range(0.0f, 100.0f)]
    float maxAcceleration = 10.0f;

    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //The following code was taken from Jasper Flick: https://catlikecoding.com/unity/tutorials/movement/sliding-a-sphere/
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal"); //GetAxis provides a smooth transition between values, giving a sense of momentum.
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f); //This is prevent diagonal movement being faster than single direction movement.

        Vector3 acceleration = new Vector3(playerInput.x, 0.0f, playerInput.y) * maxSpeed;

        //velocity = velocity + acceleration * Time.deltaTime;
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        /*
        if (velocity.x < desiredVelocity.x)
        {
            velocity.x = Mathf.Min(velocity.x + maxSpeedChange, desiredVelocity.x);
        }
        else if (velocity.x > desiredVelocity.x)
        {
            velocity.x =
                Mathf.Max(velocity.x - maxSpeedChange, desiredVelocity.x);
        }
        */

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        Vector3 displacement = velocity * Time.deltaTime;

        transform.localPosition = transform.localPosition + displacement;
    }
}
