using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    // Player controls
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
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
}