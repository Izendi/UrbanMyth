using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    [SerializeField, Range(0.0f, 100.0f)]
    float maxSpeed = 10.0f;

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

        Vector3 velocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        Vector3 displacement = velocity * Time.deltaTime;

        transform.localPosition = transform.localPosition + displacement;
    }
}
