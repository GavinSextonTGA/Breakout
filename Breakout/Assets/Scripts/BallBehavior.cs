using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private float _launchForce = 7.0f;
    [SerializeField] private float _speedIncrement = 1.05f;
    private Rigidbody2D _rb;

    [SerializeField, Range(0.0f, 1.0f)] float _paddleInfluence = 0.4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        // make sure that direction has a vector length of 1
        Vector2 direction = Random.insideUnitCircle.normalized; 

        if (Mathf.Abs(direction.y) > 0.35f)  
        direction.y += 0.35f * Mathf.Sign(direction.y); 
        direction = direction.normalized;  
        _rb.AddForce(direction * _launchForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bar"))
        {
            // Check if the paddle is moving
            if(!Mathf.Approximately(other.rigidbody.linearVelocity.x, 0.0f))
            {
                Vector2 direction =_rb.linearVelocity *(1.0f - _paddleInfluence)
                                + other.rigidbody.linearVelocity * _paddleInfluence;


                _rb.linearVelocity = _rb.linearVelocity.magnitude * direction.normalized * _speedIncrement;
            }
        }
    }

}
