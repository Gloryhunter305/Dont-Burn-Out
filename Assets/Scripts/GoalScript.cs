using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float scanRadius = 0.5f;
    private int playerCount = 0;

    public Color denyEntryColor = Color.magenta;

    [SerializeField] private GameManager spawner;

    void Start()
    {
        spawner = FindFirstObjectByType<GameManager>();   
    }

    void Update()
    {
        if (!spawner.getGameStart())
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            if (spawner.coinsCollected >= GameManager.REQUIRED_COINS)
            {
                spriteRenderer.color = Color.green;
            }   
            else
            {
                spriteRenderer.color = denyEntryColor;
            }
        }
        
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;

            if (playerCount == 2 && spawner.coinsCollected >= GameManager.REQUIRED_COINS)       //AND REQUIRED COINS ARE COLLECTED WHICH THEN I MIGHT WANT TO TEXT SHOW IT 
            {
                Debug.Log("Both players are touching the goal!!");
                ScoreManager.Instance.Score = spawner.score;

                spawner.setGameState(true); //Sets gameState to Victory
                ScoreManager.Instance.State = spawner.victoryOrNot;

                SceneManager.LoadScene("EndScene");
            }
            // else if (playerCount == 2 && spawner.coinsCollected < GameManager.REQUIRED_COINS)
            // {
            //     ScoreManager.Instance.Score = spawner.score;

            //     spawner.setGameState(false);
            //     ScoreManager.Instance.State = spawner.victoryOrNot;

            //     SceneManager.LoadScene("EndScene");
            // }
        }
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount--;
        }
    }

    void OnDrawGizmosSelected()
    {
        //Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, scanRadius);
    }

}