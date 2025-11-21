using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Health Bar")]
    public GameObject healthBarPrefab;
    private PlayerHealthBar healthBar;

    [Header("Damage Cooldown")]
    public float damageCooldown = 0.5f; // half-second invulnerability
    private float lastDamageTime = -999f;

    void Start()
    {
        currentHealth = maxHealth;

        // Spawn health bar
        if (healthBarPrefab != null)
        {
            GameObject barObj = Instantiate(
                healthBarPrefab, 
                transform.position + Vector3.up * 2f, 
                Quaternion.identity
            );

            healthBar = barObj.GetComponent<PlayerHealthBar>();
            if (healthBar != null)
            {
                healthBar.Initialize(this.transform);
                healthBar.SetHealth(1f);
                barObj.SetActive(true);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        // Prevent rapid hits
        if (Time.time - lastDamageTime < damageCooldown)
            return;

        lastDamageTime = Time.time;

        currentHealth -= amount;

        // update health bar
        if (healthBar != null)
            healthBar.SetHealth((float)currentHealth / maxHealth);

        Debug.Log($"Player took {amount} damage! HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Player DIED!");

        // Remove health bar
        if (healthBar != null)
            Destroy(healthBar.gameObject);

        // Delete the actual player
        Destroy(gameObject);
    }
}
