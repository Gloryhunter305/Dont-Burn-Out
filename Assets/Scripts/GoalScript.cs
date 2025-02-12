using UnityEngine;

public class GoalScript : MonoBehaviour
{
    private int playerCount = 0;

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;

            if (playerCount == 2)
            {
                Debug.Log("Both players are touching the goal!!");
            }
        }
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount--;
        }
    }
}