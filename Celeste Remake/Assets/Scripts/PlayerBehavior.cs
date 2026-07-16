// using UnityEngine;

// public class PlayerBehavior : MonoBehaviour
// {
//     [SerializeField] private float _moveSpeed;  
//     [SerializeField] private float _accelerate_x = 8.0f;  
//     [SerializeField] public KeyCode _rightDirection;
//     [SerializeField] public KeyCode _leftDirection;
//     private Rigidbody2D _rb;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//       _rb = GetComponent<Rigidbody2D>();  
//     }
//     private void FixedUpdate()
//     {
//         if (Input.GetKey(_rightDirection))
//             {
//             while (Mathf.Abs(_rb.linearVelocityX) < _moveSpeed){
//                 _rb.linearVelocityX += _accelerate_x;
//             }

//             }

//         if (Input.GetKey(_leftDirection))
//             {
//                 while (Mathf.Abs(_rb.linearVelocityX) < _moveSpeed){
//                 _rb.linearVelocityX -= _accelerate_x;
//             }        

//             }
                
//     }

//     // Update is called once per frame
//     void Update()
//     {


//     }
// }
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private float _direction =0.0f;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _turnAcceleration;
    [SerializeField] private float _decceleration;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _jumpHeight;
    private BoxCollider2D boxCollider2D;

    [SerializeField] public KeyCode _rightDirection;
    [SerializeField] public KeyCode _leftDirection;
    [SerializeField] public KeyCode _jump;

    private Rigidbody2D _rb;
    

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
            if (_direction != 0)
        {
        // If velocity and input have opposite signs, we're turning around.
            float acceleration = (_rb.linearVelocityX * _direction < 0)
                ? _turnAcceleration
                : _acceleration;
            // float acceleration;
            // non ternary operator version:
            // if (_rb.linearVelocityX * _direction < 0)
            //     {
            //         acceleration = _turnAcceleration;
            //     }
            // else
            // {
            //     acceleration = _acceleration;
            // }    

         _rb.linearVelocityX += acceleration * _direction * Time.fixedDeltaTime;

            _rb.linearVelocityX = Mathf.Clamp(
                _rb.linearVelocityX,
                -_moveSpeed,
                _moveSpeed
            );
        }
        else
        {
            _rb.linearVelocityX = Mathf.MoveTowards(
                _rb.linearVelocityX,
                0f,
                _decceleration * Time.fixedDeltaTime
            );
        }


    }
    void Update()
    {
        _direction = 0.0f;
        if (Input.GetKey(_rightDirection))
            _direction += 1.0f;
        if (Input.GetKey(_leftDirection))
            _direction -= 1.0f;
        if (Input.GetKey(_jump) && _isGrounded)
        {
            _rb.linearVelocityY = _jumpHeight;
        }
    }
}