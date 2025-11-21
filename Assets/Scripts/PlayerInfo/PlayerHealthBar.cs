using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image healthFill;
    private Transform target;
    private Vector3 offset = new Vector3(0, 2f, 0); // adjust for player height

    public void Initialize(Transform followTarget)
    {
        target = followTarget;
    }

    public void SetHealth(float normalizedValue)
    {
        healthFill.fillAmount = normalizedValue;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Follow the player
        transform.position = target.position + offset;

        // Always face camera
        if (Camera.main != null)
            transform.LookAt(Camera.main.transform);
    }
}
