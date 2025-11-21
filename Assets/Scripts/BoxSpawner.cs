using UnityEngine;
using System.Collections;

public class BoxSpawner : MonoBehaviour
{
    private static BoxSpawner instance;

    void Awake()
    {
        instance = this;
    }

    public static void Respawn(GameObject prefab, Vector3 pos, Quaternion rot, float delay)
    {
        if (prefab == null)
        {
            Debug.LogError("[Spawner] Cannot respawn: prefab is NULL!");
            return;
        }

        instance.StartCoroutine(instance.DoRespawn(prefab, pos, rot, delay));
    }

    private IEnumerator DoRespawn(GameObject prefab, Vector3 pos, Quaternion rot, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (prefab == null)
        {
            Debug.LogError("[Spawner] Cannot respawn: prefab is NULL or destroyed.");
            yield break;
        }

        Instantiate(prefab, pos, rot);
        Debug.Log("[Spawner] Respawned " + prefab.name);
    }
}
