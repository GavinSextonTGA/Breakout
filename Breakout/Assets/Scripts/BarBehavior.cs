using UnityEngine;

public class BarBehavior : MonoBehaviour
{
    // public float MaxSpeed;
    // public float AccelerationSpeed;
    // public float DecelerationSpeed;
    // I was curious about trying this but it will probably be explained later on so I'll just sit tight
    public float CurrentSpeed = 5.0f;
    public KeyCode LeftDirection;
    public KeyCode RightDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(LeftDirection))
        {
                movement.x -= CurrentSpeed;
        }
              if (Input.GetKey(RightDirection))
        {
                movement.x += CurrentSpeed;
        }
        movement *= Time.deltaTime;
             // Apply Movement
        transform.position += movement;
    }
}
