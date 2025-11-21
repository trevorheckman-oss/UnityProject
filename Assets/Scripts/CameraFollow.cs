using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Distance")]
    public float defaultDistance = 6f;
    public float minDistance = 2f;
    public float maxDistance = 12f;
    public float zoomSpeed = 5f;

    [Header("Height & Offset")]
    public float height = 2f;
    public float shoulderOffset = 1.5f;

    [Header("Mouse Sensitivity")]
    public float mouseSensitivityX = 300f;
    public float mouseSensitivityY = 200f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    [Header("Smooth")]
    public float positionSmoothTime = 0.1f;
    public float rotationSmoothTime = 0.1f;

    private float yaw = 0f;
    private float pitch = 20f;
    private float currentDistance;
    private Vector3 currentVelocity;
    private Vector3 currentRotationVelocity;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow: No target assigned!");
            enabled = false;
            return;
        }

        currentDistance = defaultDistance;
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        HandleMouseInput();
        HandleZoom();
        MoveCamera();
    }

    private void HandleMouseInput()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
    }

    private void MoveCamera()
    {
        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Calculate offset (shoulder + distance + height)
        Vector3 offset = rotation * new Vector3(shoulderOffset, 0, -currentDistance) + Vector3.up * height;
        Vector3 desiredPosition = target.position + offset;

        // Smooth camera position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, positionSmoothTime);

        // Smoothly look at the player
        Vector3 lookTarget = target.position + Vector3.up * 1.5f;
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothTime);
    }
}
