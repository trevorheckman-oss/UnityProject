using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxTrigger : MonoBehaviour
{
    public float speedBoost = 5f;
    public float jumpBoost = 5f;
    public float duration = 30f;

    public string prefabName = "boostblock"; // prefab filename in Resources/Prefabs
    public float respawnDelay = 15f;

    private static Dictionary<PlayerController, List<Boost>> activeBoosts
        = new Dictionary<PlayerController, List<Boost>>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                player.StartCoroutine(ApplyBoost(player, speedBoost, jumpBoost, duration));

            // ✅ Load prefab safely from Resources
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);
            if (prefab != null)
                BoxSpawner.Respawn(prefab, transform.position, transform.rotation, respawnDelay);
            else
                Debug.LogError("[BoxTrigger] Could not load prefab: " + prefabName);

            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyBoost(PlayerController player, float speed, float jump, float dur)
    {
        if (!activeBoosts.ContainsKey(player))
            activeBoosts[player] = new List<Boost>();

        Boost newBoost = new Boost(speed, jump);
        activeBoosts[player].Add(newBoost);

        player.moveSpeed += speed;
        player.jumpForce += jump;

        yield return new WaitForSeconds(dur);

        activeBoosts[player].Remove(newBoost);
        player.moveSpeed -= speed;
        player.jumpForce -= jump;

        if (activeBoosts[player].Count == 0)
            activeBoosts.Remove(player);
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
