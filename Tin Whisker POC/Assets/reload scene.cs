using UnityEngine.SceneManagement;

    void ReloadScene()
{
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(currentSceneIndex);
}