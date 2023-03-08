using UnityEngine.SceneManagement;
using UnityEngine;

public class RestartScene : MonoBehaviour
{
    public void RestartCurrentScene()
    {
        // Get the name of the current scene
        string sceneName = SceneManager.GetActiveScene().name;

        // Log the scene name to the console for debugging purposes
        Debug.Log("Reloading scene: " + sceneName);

        // Reload the current scene without resetting it
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
