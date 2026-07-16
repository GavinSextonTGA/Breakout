using UnityEngine;

public class BarBehavior : MonoBehaviour
{
    private float _direction =0.0f;
    [SerializeField] private float _currentSpeed = 8.0f;    
    [SerializeField] public KeyCode _rightDirection;
    [SerializeField] public KeyCode _leftDirection;
    private Rigidbody2D _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      _rb = GetComponent<Rigidbody2D>();  
    }
    private void FixedUpdate()
    {
        _rb.linearVelocityX = _direction *_currentSpeed;
    }
    // Update is called once per frame
    void Update()
    {
         _direction = 0.0f;

    if (GameBehavior.Instance.State == Utilities.GameState.Play) {
        if (Input.GetKey(_rightDirection))
            _direction += 1.0f;
        if (Input.GetKey(_leftDirection))
            _direction -= 1.0f;
        }

    }
}
