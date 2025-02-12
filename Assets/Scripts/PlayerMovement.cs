using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    //Gameobject Components
    public float moveSpeed = 5f;

    //Private variables (Finding their Game Components)
    private Rigidbody2D rb;
    private Light2D torch;

    // Player controls
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    // Torch Variable Timer
    public float maxBrightness = 30f;
    public float minBrightness = 0f;
    public float falloffStrength = 1f;      //Falloff starts at 0 and then increases by 1
    public float duration = 60f;    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        torch = GetComponentInChildren<Light2D>();

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
        Move();

        BurnOut();
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
        // Calculate a normalized time value
        float normalizedTime = Time.time / duration;

        //Normalize
        normalizedTime = Mathf.Clamp01(normalizedTime);

        // Decrease intensity
        torch.intensity = Mathf.Lerp(maxBrightness, minBrightness, normalizedTime);

        // Decrease falloff
        torch.falloffIntensity = Mathf.Lerp(0, falloffStrength, normalizedTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Collected coin.");
            Destroy(other.gameObject);
        }
    }
}