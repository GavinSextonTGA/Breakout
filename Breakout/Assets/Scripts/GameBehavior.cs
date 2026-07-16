using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    
    [SerializeField] private GameObject _ballPrefab;
    public static int Score;
    public Utilities.GameState State;



    
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


    void Start()
    {
       State = Utilities.GameState.Play; 

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P)) {
		// The ternary operator avoids usage of nested conditionals
		State = State == Utilities.GameState.Play ?
						Utilities.GameState.Pause :
						Utilities.GameState.Play;
}
    }




}
