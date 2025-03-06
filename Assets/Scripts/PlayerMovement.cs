using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{

    //Gameobject Components
    public float moveSpeed = 5f;
    public float playerNoNoSquare = 2f;

    //Private variables (Finding their Game Components)
    private Rigidbody2D rb;
    private Light2D torch;
    private LightSource lightsource;

    // Player controls
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    // Torch Variable Timer
    public float maxBrightness = 30f;   
    public float dischargeRate = 1.5f;
    public float rechargeRate = 1.5f;

    [Header("Private Manager")]
    [SerializeField] private GameManager manager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        torch = GetComponentInChildren<Light2D>();
        lightsource = GetComponentInChildren<LightSource>();
        manager = FindFirstObjectByType<GameManager>();   

        
        if (torch == null)
        {
            Debug.LogError("Light2D component not found on this GameObject.");
        }
        else
        {
            // Set the initial intensity and falloff values
            torch.intensity = maxBrightness;
            torch.falloffIntensity = 0;
        }
    }

    void Update()
    {
        if (manager.getGameStart())
        {
            Move();
            BurnOut();
        }
    }

    void Move()
    {
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(upKey))
        {
            movement.y += moveSpeed;
        }
        if (Input.GetKey(downKey))
        {
            movement.y -= moveSpeed;
        }
        if (Input.GetKey(leftKey))
        {
            movement.x -= moveSpeed;
        }
        if (Input.GetKey(rightKey))
        {
            movement.x += moveSpeed;
        }

        rb.linearVelocity = movement;
    }

    void BurnOut()
    {
        if (lightsource.torchTouching)
        {
            torch.intensity += Time.deltaTime * rechargeRate;
            torch.falloffIntensity -= Time.deltaTime * rechargeRate;
        }
        else
        {
            torch.intensity -= Time.deltaTime * dischargeRate;
            torch.falloffIntensity += Time.deltaTime * dischargeRate;  
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            CollectibleScript coin = other.gameObject.GetComponent<CollectibleScript>();
            coin.GetBumped();
        }
    }

    void OnDrawGizmosSelected()
    {
        //Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, playerNoNoSquare);
    }
}