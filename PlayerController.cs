using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed = 5f;

    [Header("References")]
    [Tooltip("Drag the child GameObject that contains the player's sprite here.")]
    public Transform playerSprite;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movementInput;
    private Camera mainCamera;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (playerSprite == null)
        {
            Debug.LogError("CRITICAL ERROR: Player Sprite transform is not assigned in the PlayerController script! The player will not rotate. Please drag the child sprite object into the 'Player Sprite' slot in the Inspector.");
        }
        if (mainCamera == null)
        {
             Debug.LogError("CRITICAL ERROR: No camera found with the 'MainCamera' tag! The player will not be able to aim.");
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movementInput = new Vector2(moveX, moveY).normalized;

        if (playerSprite != null && mainCamera != null)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            Vector2 aimDirection = (mousePosition - transform.position);

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            playerSprite.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        animator.SetFloat("Speed", movementInput.magnitude);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }
}

