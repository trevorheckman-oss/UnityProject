using UnityEngine;
using System.Collections.Generic;

public class Sword : MonoBehaviour
{
    public int damage = 25;

    // Track enemies hit this swing
    private HashSet<GameObject> alreadyHit = new HashSet<GameObject>();

    [Header("Assign your sword collider here")]
    public Collider swordCollider;

    private void Awake()
    {
        // Ensure collider starts disabled
        if (swordCollider != null)
            swordCollider.enabled = false;
        else
            Debug.LogWarning("Sword collider not assigned! Assign it in the Inspector.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (swordCollider == null || !swordCollider.enabled) return; // Only damage while swinging

        if (other.CompareTag("Enemy") && !alreadyHit.Contains(other.gameObject))
        {
            alreadyHit.Add(other.gameObject);

            TargetDummy dummy = other.GetComponent<TargetDummy>();
            if (dummy != null)
            {
                dummy.TakeDamage(damage);
                Debug.Log($"Hit {other.name} for {damage} damage!");
            }
            else
            {
                Debug.LogWarning($"{other.name} does not have TargetDummy script!");
            }
        }
    }

    // Call at the start of each swing
    public void ResetHits()
    {
        alreadyHit.Clear();
        Debug.Log("Sword hits reset.");
    }

    // Animation Events: Enable collider at start of swing
    public void EnableCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
            Debug.Log("Sword collider enabled.");
        }
        else
        {
            Debug.LogError("EnableCollider called but swordCollider is null!");
        }
    }

    // Animation Events: Disable collider at end of swing
    public void DisableCollider()
    {
        if (swordCollider != null)
            swordCollider.enabled = false;
        Debug.Log("Sword collider disabled.");
    }
}
