using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickReset : MonoBehaviour
{
    // Name of the first level
    public string firstLevelName = "level1";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Load Level1 no matter which scene we're currently in
            SceneManager.LoadScene(firstLevelName);
        }
    }
}
