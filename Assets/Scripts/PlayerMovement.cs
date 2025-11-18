using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 8f;
    public float gravity = -25f;
    public float coyoteTime = 0.2f;
    private Vector3 velocity;
    private bool isGrounded;
    private float lastGroundedTime;

    [Header("Double Jump")]
    private int extraJumps;
    public int maxExtraJumps = 1;

    [Header("Player Rotation")]
    public bool rotatePlayerToMove = true;
    public float playerRotationSpeed = 10f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        extraJumps = maxExtraJumps;
    }

    void Update()
    {
        HandleMovement();
    }

    // ---------- Movement ----------
    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            lastGroundedTime = Time.time;
            extraJumps = maxExtraJumps;
        }

        // --- Camera-relative movement ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * z + right * x);
        if (move.magnitude > 1f) move.Normalize();

        // --- Apply movement ---
        controller.Move(move * moveSpeed * Time.deltaTime);

        // --- Rotate player toward movement direction ---
        if (rotatePlayerToMove && move.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, playerRotationSpeed * Time.deltaTime);
        }

        // --- Jump ---
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || Time.time - lastGroundedTime <= coyoteTime)
            {
                velocity.y = jumpForce;
            }
            else if (extraJumps > 0)
            {
                velocity.y = jumpForce;
                extraJumps--;
            }
        }

        // --- Gravity ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
