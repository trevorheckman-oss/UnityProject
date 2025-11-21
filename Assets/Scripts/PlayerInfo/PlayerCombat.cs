using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator swordAnimator; // assign SwordPivot Animator here
    public Sword sword;            // assign your existing Sword script
    private Camera mainCamera;

    void Awake()
    {
        if (Camera.main != null)
            mainCamera = Camera.main;
        else
            Debug.LogError("No main camera found! Make sure your camera is tagged 'MainCamera'");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RotateTowardMouse();

            // Step 1: Reset hits so the sword can hit enemies again
            sword.ResetHits();

            // Step 2: Trigger attack animation
            swordAnimator.SetTrigger("Swing");
        }
    }

    void RotateTowardMouse()
    {
        if (mainCamera == null) return;

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 direction = (hitPoint - transform.position).normalized;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
