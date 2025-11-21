using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TargetDummy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Transform player;
    public float detectionRange = 10f;
    public float moveSpeed = 3f;
    public float stopDistance = 1f;

    public int contactDamage = 10;

    [Header("Health Bar")]
    public GameObject healthBarPrefab;
    private EnemyHealthBar healthBar;

    private Rigidbody rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

        if (player == null)
            Debug.LogError("Player not assigned on TargetDummy!");

        if (healthBarPrefab != null)
        {
            GameObject barObj = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            healthBar = barObj.GetComponent<EnemyHealthBar>();
            if (healthBar != null)
            {
                healthBar.Initialize(this.transform);
                healthBar.SetHealth(1f);
                barObj.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector3 direction = player.position - rb.position;
        direction.y = 0;
        float distance = direction.magnitude;

        // Show the health bar only when player is close
        if (healthBar != null)
            healthBar.gameObject.SetActive(distance <= detectionRange);

        // Move toward the player, but DO NOT deal damage from range
        if (distance <= detectionRange && distance > stopDistance)
        {
            Vector3 move = direction.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
            rb.MoveRotation(Quaternion.LookRotation(direction));
        }
    }

    // ======== CONTACT DAMAGE ONLY ========
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            DealCollisionDamage(collision.collider);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            DealCollisionDamage(other);
    }

    void DealCollisionDamage(Collider col)
    {
        PlayerHealth hp = col.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(contactDamage);
            Debug.Log($"{name} dealt {contactDamage} collision damage to Player!");
        }
    }
    // ====================================

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
            healthBar.SetHealth((float)currentHealth / maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (healthBar != null)
            Destroy(healthBar.gameObject);

        Destroy(gameObject);
    }
}
