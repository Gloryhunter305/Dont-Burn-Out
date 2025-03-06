using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour
{
    public Camera mainCam; 
    public Color defeatBackground = Color.black;

    public TextMeshPro gameStateText;
    public TextMeshPro scoreText;
    public TextMeshPro authorComment;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ScoreManager.Instance.State)
        {
            gameStateText.text = "Victory!!";
            scoreText.text = "Score: " + ScoreManager.Instance.Score.ToString();
        }
        else
        {
            mainCam.backgroundColor = defeatBackground;
            gameStateText.text = "Defeat...";
            scoreText.text = "Score: -" + ScoreManager.Instance.Score.ToString();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("GameScene");
            GameManager.Instance.ResetScene();
        }
    }
}
