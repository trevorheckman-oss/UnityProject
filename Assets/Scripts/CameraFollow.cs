using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float cameraSmoothSpeed = 50f;

    // Fixed horizontal offset (X,Z) - keeps diagonal view
    public Vector3 baseOffset = new Vector3(-10, 10, 10);

    // Vertical adjustment with mouse
    public float verticalSpeed = 5f;
    public float minHeight = 2f;   // lower limit
    public float maxHeight = 30f;  // upper limit

    private float verticalOffset;

    void Start()
    {
        verticalOffset = baseOffset.y; // start at default height
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Mouse movement changes vertical offset
        float mouseY = Input.GetAxis("Mouse Y");
        verticalOffset -= mouseY * verticalSpeed * Time.deltaTime;
        verticalOffset = Mathf.Clamp(verticalOffset, minHeight, maxHeight);

        // Camera position = fixed X,Z + adjustable Y
        Vector3 desiredPosition = target.position + new Vector3(baseOffset.x, verticalOffset, baseOffset.z);

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);

        // Always look at player's chest
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
