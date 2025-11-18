using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage = 25;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Sword collided with {other.name} (tag: {other.tag})");

        // Only affect objects tagged "Enemy"
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected!");

            // Try to get the TargetDummy or Enemy script
            TargetDummy dummy = other.GetComponent<TargetDummy>();
            if (dummy != null)
            {
                Debug.Log($"Applying {damage} damage to {other.name}");
                dummy.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning($"{other.name} does not have TargetDummy script!");
            }
        }
        else
        {
            Debug.Log($"{other.name} is not tagged as Enemy, ignoring");
        }
    }
}
