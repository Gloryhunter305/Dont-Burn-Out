using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    //Saving score to transfer to other scene
    public int score = 0;
    
    [Header("SpawnManager Variables")]
    public GameObject gold_Coins;
    public const int REQUIRED_COINS = 12;
    public int coinsInMaze = 10;
    public int coinsCollected = 0;

    [Header("Scoring Variables")]
    public const int COIN_SCORE = 10;

    [Header("Game Start variables")]
    [SerializeField] private float normalizedTime = 0f;
    public float duration = 60f;

    public Light2D globalLight;
    private bool gameStart = false, gameReset = false;

    [Header("Private variables")]
    [SerializeField] private List<GameObject> activeCoins = new List<GameObject>();
    
    public bool victoryOrNot = false;

    [Header("Gameobject's Positions")]
    private GameObject[] players;
    public GameObject coins;
    public GameObject endGoal;
    public LayerMask layerMask;

    private bool canMove = false;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        globalLight = FindFirstObjectByType<Light2D>();
        players = GameObject.FindGameObjectsWithTag("Player");
        endGoal = GameObject.FindGameObjectWithTag("Goal");

        //Before starting the game, reset everything first
        ResetScene();
        //Begin Coroutines here
        StartCoroutine(LoadGame());
    }

    // Update is called once per frame
    void Update()
    {
        //Game does not start yet (Players can see the layout of maze, but cannot see themselves in the maze)
        if (!gameStart) 
        {
            normalizedTime = Time.time / duration;
            normalizedTime = Mathf.Clamp01(normalizedTime);
            globalLight.intensity = Mathf.Lerp(1, 0, normalizedTime);       //Do not use Lerp
        }

        if (globalLight.intensity == 0)
        {
            gameStart = true;
        }
        
        if (gameReset)
        {
            ResetScene();
            gameReset = false;
        }
    }

    //Player's GetRandomPosition();
    Vector2 GetRandomPlayerPosition()
    {
        //Dimensions of the game arena
        float randomX = Random.Range(-9.5f, 9.5f);
        float randomY = Random.Range(-5, 5f);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        return spawnPosition;
    }

    bool IsSafeSpace(Vector2 position, float withinItsRadar)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, withinItsRadar, layerMask);

        return hits.Length == 0;
    }

    private IEnumerator LoadGame()
    {
        canMove = false;

        yield return new WaitForSeconds(duration);

        TeleportPlayersIntoMaze();

        //Spawn goal and all collectibles
        SpawnCollectiblesInMaze();
        SpawnGoalInMaze();

        canMove = true;
    }
    private void TeleportPlayersIntoMaze()
    {
        players[0].transform.position = GetRandomPlayerPosition();
        players[1].transform.position = GetRandomPlayerPosition();
    }

    private void SpawnCollectiblesInMaze()  //Spawning 10 coins within the maze dimensions 
    {   
        for (int i = activeCoins.Count; i < coinsInMaze; i++)
        {
            PlayerMovement player1 = players[0].GetComponent<PlayerMovement>();
            PlayerMovement player2 = players[1].GetComponent<PlayerMovement>();
            CollectibleScript collectibleScript = coins.GetComponent<CollectibleScript>();

            //Get both players' radius and position
            float radius1 = player1.playerNoNoSquare;
            float radius2 = player2.playerNoNoSquare;
            Vector2 player1Pos = players[0].transform.position;
            Vector2 player2Pos = players[1].transform.position;
            //Getting collectibles scanRadius
            float coinsRadius = collectibleScript.scanRadius;


            Vector2 spawnPos = GetRandomPositionInMaze(player1Pos, player2Pos, radius1, radius2, coinsRadius);
            GameObject collectible = Instantiate(gold_Coins, spawnPos, Quaternion.identity);

            activeCoins.Add(collectible);

        }
    }
    private void SpawnGoalInMaze()      //Spawn one goal within the maze. Code is doable.
    {
        PlayerMovement player1 = players[0].GetComponent<PlayerMovement>();
        PlayerMovement player2 = players[1].GetComponent<PlayerMovement>();
        CollectibleScript collectibleScript = coins.GetComponent<CollectibleScript>();
        GoalScript goalScript = endGoal.GetComponent<GoalScript>();

        //Get both players' radius and position
        float radius1 = player1.playerNoNoSquare;
        float radius2 = player2.playerNoNoSquare;
        Vector2 player1Pos = players[0].transform.position;
        Vector2 player2Pos = players[1].transform.position;
        float coinsRadius = collectibleScript.scanRadius;
        float goalRadius = goalScript.scanRadius;

        //For goal
        endGoal.transform.position = GetRandomPositionInMaze(player1Pos, player2Pos, radius1, radius2, coinsRadius, goalRadius);
    }

    //For Collectibles
    Vector2 GetRandomPositionInMaze(Vector2 position1, Vector2 position2, float radi1, float radi2, float scanRadi)
    {
        //Dimensions of the game arena
        float randomX = Random.Range(-9.5f, 9.5f);
        float randomY = Random.Range(-5, 5f);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        //If spawnPosition is within the player's scanRadius re-randomize enemy's spawn point
        if (Vector2.Distance(spawnPosition, position1) < radi1 || Vector2.Distance(spawnPosition, position2) < radi2)
        {
            return GetRandomPositionInMaze(position1, position2, radi1, radi2, scanRadi);
        }

        if (!IsSafeSpace(spawnPosition, scanRadi))
        {
            return GetRandomPositionInMaze(position1, position2, radi1, radi2, scanRadi);
        }
        
        return spawnPosition;
    }

    //Exclusive for Goal To Use
    Vector2 GetRandomPositionInMaze(Vector2 position1, Vector2 position2, float radi1, float radi2, float scanRadi, float coinRadi)
    {
        //Dimensions of the game arena
        float randomX = Random.Range(-9.5f, 9.5f);
        float randomY = Random.Range(-5, 5f);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        //If spawnPosition is within the player's scanRadius re-randomize enemy's spawn point
        if (Vector2.Distance(spawnPosition, position1) < radi1 || Vector2.Distance(spawnPosition, position2) < radi2)
        {
            return GetRandomPositionInMaze(position1, position2, radi1, radi2, scanRadi, coinRadi);
        }

        //If spawnPosition within the collectible's scanRadius and within coinRadius??
        if (!IsSafeSpace(spawnPosition, scanRadi) && !IsSafeSpace(spawnPosition, coinRadi))
        {
            return GetRandomPositionInMaze(position1, position2, radi1, radi2, scanRadi, coinRadi);
        }
        
        return spawnPosition;
    }

    public void TargetHit(GameObject coin)
    {
        activeCoins.Remove(coin);
        //Collect coins and increase score
        coinsCollected++;
        score += COIN_SCORE;

        if (activeCoins.Count <= 4)
        {
            SpawnCollectiblesInMaze();
        }
    }

    public bool getCanMove()
    {
        return canMove;
    }

    public bool getGameStart()
    {
        return gameStart;
    }

    public void setGameState(bool YesOrNo)
    {
        victoryOrNot = YesOrNo;
        gameReset = true;
    }

    public void ResetScene()
    {
        Debug.Log("Game has resetted");
        score = 0;
        coinsCollected = 0;
        gameStart = false;
        canMove = false;
        globalLight.intensity = Mathf.Lerp(0, 1, .5f);      //Do not use Lerp because it may cause some
        players[0].transform.position = new Vector2(-11.75f, 4.75f);
        players[1].transform.position = new Vector2(-11.75f, 1.25f);
        endGoal.transform.position = new Vector2(-11.75f, -2f);
        activeCoins.Clear();
    }
}
