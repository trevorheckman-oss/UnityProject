using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TargetDummy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Transform player;      // Assign your Player here in Inspector
    public float moveSpeed = 3f;  // Units per second
    public float stopDistance = 1f; // How close to get before stopping

    private Rigidbody rb;         // <-- Add this

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();  // <-- Assign it

        Debug.Log($"{gameObject.name} initialized with {maxHealth} HP");

        if (player == null)
            Debug.LogError("Player not assigned on TargetDummy!");
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector3 direction = (player.position - rb.position);
        direction.y = 0; // optional, stay on ground
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            Vector3 move = direction.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);

            // Optional: rotate to face player
            rb.MoveRotation(Quaternion.LookRotation(direction));
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage! Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} destroyed!");
        Destroy(gameObject);
    }
}
