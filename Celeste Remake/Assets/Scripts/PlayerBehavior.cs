using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum PlayerState
    {
        Normal,
        Dash
    }

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _acceleration = 40f;
    [SerializeField] private float _turnAcceleration = 70f;
    [SerializeField] private float _deceleration = 50f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _gravity = 70f;
    [SerializeField] private float _jumpGravity = 45f;
    [SerializeField] private float _maxFallSpeed = 22f;
    [SerializeField] private float _wallJumpX = 12f;
    [SerializeField] private float _wallJumpY = 12f;
    [SerializeField] private float _jumpBufferTime = 0.1f;


    [Header("Dash")]
    [SerializeField] private float _dashSpeed = 18f;
    [SerializeField] private float _dashDuration = 0.15f;
    public int _maxDashes;
    [SerializeField] private int _boost;
    [SerializeField] private float _dashJumpWindow = 0.25f;

    
    [Header("Forgiveness")]
    [SerializeField] private float _coyoteTime = 0.12f;
    // [SerializeField] private float _cornerCorrection = 0.15f;




    [Header("Input")]
    [SerializeField] private KeyCode _rightDirection = KeyCode.RightArrow;
    [SerializeField] private KeyCode _leftDirection = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _upDirection = KeyCode.UpArrow;
    [SerializeField] private KeyCode _downDirection = KeyCode.DownArrow;

    [SerializeField] private KeyCode _jump = KeyCode.C;
    [SerializeField] private KeyCode _dash = KeyCode.X;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.05f;
    [SerializeField] private float _wallCheckDistance = 0.05f;
    [SerializeField] private float _wallJumpLockTime = 0.2f; // how long to lock after wall jump


    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sprite;

    private PlayerState _state = PlayerState.Normal;

    private Vector2 velocity;
    private bool _onWall;
    private int _wallDirection;

    private bool _grounded;
    private bool _canBoostJump;
    private bool _boosted;
    private bool _ceilinged;

    private int facingDirection = 1; // 1 = right, -1 = left
    private int _currentDashes;


    private Vector2 input;

    private Vector2 dashDirection;
    private float dashTimer;
    private float _dashJumpTimer;
    private float _jumpBufferTimer;
    private float _coyoteTimer;
    private float _wallJumpLock; // current countdown timer

    private bool _jumpPressed;
    private bool _dashPressed;





    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();

        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
    }


    private void Update()
    {
        ReadInput();
    }


    private void FixedUpdate()
    {

        if (_jumpBufferTimer > 0)
        {
            _jumpBufferTimer -= Time.fixedDeltaTime;
        }
        GroundCheck();
        WallCheck();
        if (_grounded)
        {
            _coyoteTimer = _coyoteTime;
        }
        else
        {
            _coyoteTimer -= Time.fixedDeltaTime;
        }
        if (_wallJumpLock > 0)
        {
            _wallJumpLock -= Time.fixedDeltaTime;
        }

        if (_grounded && velocity.y < 0f)
        {
            velocity.y = 0f;
        }
        if (_ceilinged && velocity.y > 0f)
        {
            velocity.y = 0;

            // TryCornerCorrection();
        }
        if (_dashJumpTimer > 0)
        {
            _dashJumpTimer -= Time.fixedDeltaTime;
        }

        switch (_state)
        {
            case PlayerState.Normal:
                NormalUpdate();
                break;

            case PlayerState.Dash:
                DashUpdate();
                break;
        }

  
        ApplyPhysics();

        _sprite.color = _currentDashes switch
        {
            2 => Color.limeGreen,
            1 => Color.red,
            _ => Color.cyan
        };

        _jumpPressed = false;
        _dashPressed = false;
    }


    private void ReadInput()
    {
        input = Vector2.zero;


        if(Input.GetKey(_rightDirection))
            input.x += 1;

        if(Input.GetKey(_leftDirection))
            input.x -= 1;


        if(Input.GetKey(_upDirection))
            input.y += 1;

        if(Input.GetKey(_downDirection))
            input.y -= 1;

    
        if (Input.GetKeyDown(_jump))
        {
            _jumpPressed = true;
            _jumpBufferTimer = _jumpBufferTime;
        }

        if (Input.GetKeyDown(_dash))
            _dashPressed = true;

        if (input.x > 0) 
            {
                facingDirection = 1;
            }
        else if (input.x < 0)
            {
                facingDirection = -1;
            }

    }

    private void GroundCheck()
    {
        Vector2 size = _boxCollider.bounds.size;
        size.x -= 0.05f;   // Slightly narrower than the player

        _grounded = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            size,
            0f,
            Vector2.down,
            _groundCheckDistance,
            _groundLayer 
        );
        _ceilinged = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            size,
            0f,
            Vector2.up,
            _groundCheckDistance,
            _groundLayer
        );
    }
    private void WallCheck()
    {
        Vector2 size = _boxCollider.bounds.size;
        size.y -= 0.05f;

        RaycastHit2D left = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            size,
            0,
            Vector2.left,
            _wallCheckDistance,
            _groundLayer
        );

        RaycastHit2D right = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            size,
            0,
            Vector2.right,
            _wallCheckDistance,
            _groundLayer
        );


        if(left)
        {
            _onWall = true;
            _wallDirection = -1;
        }
        else if(right)
        {
            _onWall = true;
            _wallDirection = 1;
        }
        else
        {
            _onWall = false;
        }
    }

    private void NormalUpdate()
    {


        if (_grounded)
        {
            _currentDashes = _maxDashes;
        }

        if (_dashPressed && _currentDashes > 0)
        {
            BeginDash();
            return;
        }


        if (_jumpPressed)
        {
            if (_dashJumpTimer > 0 && _canBoostJump && _grounded)
            {
                BoostJump();
                _dashJumpTimer = 0;
                _jumpBufferTimer = 0;
            }
            else if (_grounded || _coyoteTimer > 0)
            {
                Jump();
                _jumpBufferTimer = 0;
            }
            else if (_onWall)
            {
                WallJump();
                _jumpBufferTimer = 0;
            }

            _jumpPressed = false;
            // Jump buffer - recently pressed jump, now grounded → jump
            if (_jumpBufferTimer > 0 && _grounded)
            {
                Jump();
                _jumpBufferTimer = 0;
            }
        }



        HandleMovement();

        ApplyGravity();
    }



    private void HandleMovement()
    {
        if (_wallJumpLock > 0)
        {
            return;
        }

        if(input.x != 0)
        {
            float acceleration =
                Mathf.Sign(input.x) != Mathf.Sign(velocity.x)
                ? _turnAcceleration
                : _acceleration;

            velocity.x = Mathf.MoveTowards(
                velocity.x,
                input.x * _moveSpeed,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            velocity.x = Mathf.MoveTowards(
                velocity.x,
                0,
                _deceleration * Time.fixedDeltaTime
            );
        }
    }



    private void ApplyGravity()
    {
        if(velocity.y > 0)
        {
            velocity.y -= _jumpGravity * Time.fixedDeltaTime;
        }
        else
        {
            velocity.y -= _gravity * Time.fixedDeltaTime;
        }


        velocity.y = Mathf.Max(
            velocity.y,
            -_maxFallSpeed
        );
    }

    private void Jump()
    {
        velocity.y = _jumpForce;
        _coyoteTimer = 0;
        Debug.Log("Jump!");
    }
    private void BoostJump()
    {

        velocity.y = _jumpForce;
        velocity.x = _boost * facingDirection;
        _coyoteTimer = 0;
        _boosted = true;
        Debug.Log("Boost");
        _currentDashes = _maxDashes;
    }
    private void WallJump()
    {

        velocity.x = -_wallDirection * _wallJumpX;
        velocity.y = _wallJumpY;

        _wallJumpLock = _wallJumpLockTime;
    }

    // private void TryCornerCorrection()
    // {
    //     Vector2 position = transform.position;

    //     RaycastHit2D left = Physics2D.Raycast(
    //         position,
    //         Vector2.left,
    //         _cornerCorrection,
    //         _groundLayer
    //     );

    //     RaycastHit2D right = Physics2D.Raycast(
    //         position,
    //         Vector2.right,
    //         _cornerCorrection,
    //         _groundLayer
    //     );


    //     if (!left && right)
    //     {
    //         transform.position += Vector3.left * _cornerCorrection;
    //     }
    //     else if (!right && left)
    //     {
    //         transform.position += Vector3.right * _cornerCorrection;
    //     }
    // }


    private void BeginDash()
    {
        _state = PlayerState.Dash;

        dashTimer = _dashDuration;
        _dashJumpTimer = _dashJumpWindow;

        _currentDashes -= 1;

        dashDirection = input;

        if (dashDirection == Vector2.zero)
        {
            dashDirection = facingDirection == 1
                ? Vector2.right
                : Vector2.left;
        }

        dashDirection.Normalize();

        _canBoostJump = dashDirection.y <= 0.25f;

        velocity = dashDirection * _dashSpeed;
    }



    private void DashUpdate()
    {
        dashTimer -= Time.fixedDeltaTime;

        velocity = dashDirection * _dashSpeed;

        if (dashTimer <= 0f)
        {
            EndDash();
        }
    }


    private void EndDash()
    {
        _state = PlayerState.Normal;

        _boosted = false;
    }



    private void ApplyPhysics()
    {
        _rb.linearVelocity = velocity;
    }






}

