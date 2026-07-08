using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    [SerializeField]private GameObject _ballPrefab;
    public static int Score;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void SpawnBall()
    {
        Instantiate(_ballPrefab);
    }

   private void ResetGame()
    {
      
        Score = 0;
        
        SpawnBall();
    }


}
