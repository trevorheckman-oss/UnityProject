using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float gravity = -20f;
    public float coyoteTime = 0.2f;

    [Header("Jumping")]
    public int maxExtraJumps = 1;

    [Header("Rotation")]
    public float rotationSmoothTime = 0.15f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float lastGroundedTime;
    private int extraJumps;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        extraJumps = maxExtraJumps;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            lastGroundedTime = Time.time;
            extraJumps = maxExtraJumps;
        }

        // Camera-relative movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Transform cam = Camera.main.transform;
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0f; camRight.y = 0f;
        camForward.Normalize(); camRight.Normalize();

        Vector3 move = camForward * z + camRight * x;
        if (move.magnitude > 1f) move.Normalize();

        // Smooth rotation toward movement
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothTime);
        }

        // Apply movement
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Jump
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

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
