using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthFill;
    private Transform target;
    private Vector3 offset = new Vector3(0, 2f, 0);

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

        // Follow enemy
        transform.position = target.position + offset;

        // Always face player camera
        transform.LookAt(Camera.main.transform);
    }
}
