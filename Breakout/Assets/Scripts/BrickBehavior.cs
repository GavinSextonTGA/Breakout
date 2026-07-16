using Unity.VisualScripting;
using UnityEngine;

public class BrickBehavior : MonoBehaviour
{
    [SerializeField] int BrickHealth = 1;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            BrickHealth -=1; 
            
        if (BrickHealth < 1)
            {
            GameBehavior.Score += 1;
            Debug.Log(GameBehavior.Score);
            Destroy(gameObject);
            }
        }
        
    }
private void Update()
    {

    }
}
