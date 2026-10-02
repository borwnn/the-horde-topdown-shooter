using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class TopDownMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The movement speed of the player.")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput.normalized * moveSpeed;
    }

    public static class GameData
{
    public static bool ShouldLoadOnStart = false;
}

}