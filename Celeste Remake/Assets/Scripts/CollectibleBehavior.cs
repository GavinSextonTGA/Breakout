using JetBrains.Annotations;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static int count = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            count++;
            other.GetComponent<Movement>()._maxDashes = 2;
            Destroy(gameObject);
        }
    }
}
