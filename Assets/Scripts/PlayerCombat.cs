using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private Camera mainCamera;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("No Animator found on Player!");

        // Automatically assign camera
        if (Camera.main != null)
            mainCamera = Camera.main;
        else
            Debug.LogError("No main camera found! Make sure your camera is tagged 'MainCamera'");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click
        {
            RotateTowardMouse();
            animator.SetTrigger("Swing"); // trigger attack animation
        }
    }

    void RotateTowardMouse()
    {
        if (mainCamera == null) return;

        // Create a plane at Y=0 (ground level)
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        // Ray from camera through mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 direction = (hitPoint - transform.position).normalized;

            direction.y = 0; // keep rotation only in horizontal plane

            if (direction.sqrMagnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
