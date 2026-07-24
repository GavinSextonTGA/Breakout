using UnityEngine;
using TMPro;

public class GameBehavior : MonoBehaviour
{
    // Both instance and access point
    public static GameBehavior Instance;

    private Utilities.GameState _state;
    public Utilities.GameState State
    {
        get => _state;

        set
        {
            _state = value;

            _message.enabled = State == Utilities.GameState.Pause;
            Time.timeScale = State == Utilities.GameState.Play ? 1f : 0f;
        }
    }
    
    [SerializeField] private TMP_Text _message;
    





    private void Awake()
    {
        // Software Design Patterns
        // Singleton Pattern: Enforces that there is only ever one class
        // throughout the whole execution of the program
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        // Set initial state
        State = Utilities.GameState.Play;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Ternary operator
            State = State == Utilities.GameState.Play ?     // Condition
                Utilities.GameState.Pause :                 // Passing
                Utilities.GameState.Play;                   // Failing
        }
    }


    

    
}