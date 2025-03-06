using Unity.VisualScripting;
using UnityEngine;

public class LightSource : MonoBehaviour
{

    //Circle Collider decreases
    // public float duration = 60f;
    // public float acceleratedDuration = 15f; //Needs fixing
    // public float thresholdRadius = 0.75f;
    public float maxSize = 1.1f;

    public float dischargeRate;
    public float rechargeRate;

    //Track current radius of light
    // public float currentSize;
    // private float normalizedTime = 0f;

    //Bool variable to track from playerMovement script
    public bool torchTouching = false;

    CircleCollider2D myCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCollider = GetComponent<CircleCollider2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        DecreaseInSize();
    }

    private void DecreaseInSize()
    {
        if (torchTouching)
        {
            // myCollider.radius = currentSize;
            if (myCollider.radius >= maxSize)
            {
                myCollider.radius = maxSize;
            }
            else
            {
                myCollider.radius += Time.deltaTime * rechargeRate;
            }
        }
        else
        {
            // if (myCollider.radius > thresholdRadius)
            // {
            //     normalizedTime = Time.time / duration;
            // }
            // else
            // {
            //     normalizedTime = Time.time / acceleratedDuration;
            // }

            // //Normalize
            // normalizedTime = Mathf.Clamp01(normalizedTime);

            // // Decrease intensity
            // myCollider.radius = Mathf.Lerp(maxSize, 0, normalizedTime);
            // currentSize = myCollider.radius;

            myCollider.radius -= Time.deltaTime * dischargeRate;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Torch has found other player.");
            //Stops decreasing the light and it's collider
            torchTouching = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Player is now separated");
        torchTouching = false;
        DecreaseInSize();
    }
}
