using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        QuitGame();
    }

    private void QuitGame()
    {
        Debug.Log("Quit!");  // Works in editor
        Application.Quit();  // Works in build
    }
}
