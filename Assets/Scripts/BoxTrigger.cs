using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxTrigger : MonoBehaviour
{
    public float speedBoost = 5f;
    public float jumpBoost = 5f;
    public float duration = 30f;

    // Track boosts per player
    private static Dictionary<PlayerMovement, List<Boost>> activeBoosts = new Dictionary<PlayerMovement, List<Boost>>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // Apply boost from the player's MonoBehaviour
                player.StartCoroutine(ApplyBoost(player, speedBoost, jumpBoost, duration));
            }

            // Destroy the box so it disappears immediately
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyBoost(PlayerMovement player, float speed, float jump, float dur)
    {
        if (!activeBoosts.ContainsKey(player))
            activeBoosts[player] = new List<Boost>();

        Boost newBoost = new Boost(speed, jump);
        activeBoosts[player].Add(newBoost);

        // Apply immediately
        player.moveSpeed += speed;
        player.jumpForce += jump;

        // Wait for duration
        yield return new WaitForSeconds(dur);

        // Remove this specific boost
        if (activeBoosts.ContainsKey(player))
        {
            activeBoosts[player].Remove(newBoost);
            player.moveSpeed -= newBoost.speed;
            player.jumpForce -= newBoost.jump;

            if (activeBoosts[player].Count == 0)
                activeBoosts.Remove(player);
        }
    }

    private class Boost
    {
        public float speed;
        public float jump;

        public Boost(float s, float j)
        {
            speed = s;
            jump = j;
        }
    }
}
