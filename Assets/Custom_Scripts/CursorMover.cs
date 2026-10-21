using UnityEngine;
using UnityEngine.InputSystem; // 1. Important: Add this line at the top

public class CursorMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calculate screen boundaries in world points
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            screenBounds = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCam.transform.position.z));
        }

        // Get half-size of the shape so edges don't leave the screen
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            objectWidth = spriteRenderer.bounds.extents.x;
            objectHeight = spriteRenderer.bounds.extents.y;
        }
    }

    // 2. This custom public function reads the New Input System values
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Calculate next movement step
        Vector2 nextPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Clamp the position so it stays inside screen bounds
        nextPosition.x = Mathf.Clamp(nextPosition.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        nextPosition.y = Mathf.Clamp(nextPosition.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);

        rb.MovePosition(nextPosition);
    }
}