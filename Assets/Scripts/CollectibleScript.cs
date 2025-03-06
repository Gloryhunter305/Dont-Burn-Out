using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    [Header("Enemy Components")]
    public float scanRadius = 0.4f;

    [Header("Spawn Manager")]
    [SerializeField] private GameManager spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawner = GameManager.FindFirstObjectByType<GameManager>();
    }

    public void GetBumped()
    {
        spawner.TargetHit(gameObject);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, scanRadius);
    }
}
