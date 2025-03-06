using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}
    public int Score {get; set;}
    public bool State {get; set;}

    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }   
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();   
    }

    public void ResetScene()
    {
        Score = 0;
        gameManager.ResetScene();
        
    }
}
