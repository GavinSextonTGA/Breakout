using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private float _launchForce = 7.0f;
    [SerializeField] private float _speedIncrement = 1.05f;
    private Rigidbody2D _rb;
    [SerializeField] private AudioClip paddleSound;
    [SerializeField] private AudioClip wallSound;
    [SerializeField] private AudioClip brickSound;

     private AudioSource audioSource;
    [SerializeField, Range(0.0f, 1.0f)] float _paddleInfluence = 0.4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        _rb = GetComponent<Rigidbody2D>();
        ResetBall();
        audioSource = GetComponent<AudioSource>();   
 
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
            audioSource.PlayOneShot(paddleSound);
        }
        else if(other.gameObject.CompareTag("Brick"))
        {
            audioSource.PlayOneShot(brickSound);
        }
        else
        {
            audioSource.PlayOneShot(wallSound);
        }

    }
        private void ResetBall()
    {
        _rb.linearVelocity = Vector2.zero;
        transform.position = Vector3.zero;
        //transform.position = new UnityEngine.Vector3(0.0f,0.0f,transform.position.z);
        // make sure that direction has a vector length of 1
        Vector2 direction = Random.insideUnitCircle.normalized; 

        if (Mathf.Abs(direction.y) > 0.35f)  
        direction.y += 0.35f * Mathf.Sign(direction.y);   
        _rb.AddForce(direction * _launchForce, ForceMode2D.Impulse);
    }
    void Update()
    {
          _rb.simulated = GameBehavior.Instance.State == Utilities.GameState.Play;  
    }
}
