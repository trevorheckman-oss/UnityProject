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

    [Header("Camera")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 2, -5);
    public float cameraSmoothSpeed = 10f;
    public float mouseSensitivity = 2f;
    private float pitch = 0f;
    private float yaw = 0f;
    public float pitchLimit = 80f;
    public float yawLimit = 90f;

    [Header("Player Rotation")]
    public bool rotatePlayerToMove = true;
    public float playerRotationSpeed = 10f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        extraJumps = maxExtraJumps;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (cameraTransform == null) return; // safety check

        HandleMovement();
        HandleCamera();
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

        // WASD input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Camera-relative movement
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * z + right * x);
        if (move.magnitude > 1f) move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Rotate player smoothly toward movement direction
        if (rotatePlayerToMove && move.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, playerRotationSpeed * Time.deltaTime);
        }

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

    // ---------- Camera ----------
    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);
        yaw = Mathf.Clamp(yaw, -yawLimit, yawLimit);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);

        // Smooth follow
        Vector3 desiredPosition = transform.position + targetRotation * cameraOffset;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);
        cameraTransform.LookAt(transform.position);

        // Escape to unlock cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
