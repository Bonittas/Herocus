using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.CompareTag(playerTag))
        {
            Debug.Log("Player has reached the checkpoint");
            StartCoroutine(LoadSuccessScene());
        }
    }

    private IEnumerator LoadSuccessScene()
    {
        // Optional: Play an animation or effect here
        yield return new WaitForSeconds(1f); // Wait for 1 second
        LoadScene("GameSuccess");
    }

    public void StartGame()
    {
        LoadScene("StartGame");
    }

    public void LoadGameOver()
    {
        LoadScene("GameOver");
    }

    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void LoadNextLevel()
    {
        string nextLevelSceneName = "Level2"; // Replace with your logic
        LoadScene(nextLevelSceneName);
    }

public void RestartGame()
{
    Debug.Log("Restart Game button clicked"); // Debug message

    // Unload GameOver scene if it is loaded
    if (SceneManager.GetSceneByName("GameOver").isLoaded)
    {
        SceneManager.UnloadSceneAsync("GameOver");
    }

    // Load the Start Game scene
    LoadScene("StartGame");
}

private void LoadScene(string sceneName)
{
    Debug.Log($"Loading scene: {sceneName}"); // Debug message
    SceneManager.LoadScene(sceneName);
}

    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDgitITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void LoadSceneWithCameraDepth(string sceneName, int cameraDepth)
    {
        LoadScene(sceneName);
        Camera sceneCamera = Camera.main;
        if (sceneCamera != null)
        {
            sceneCamera.depth = cameraDepth;
        }
    }

    public void TransitionToScene()
    {
        LoadSceneWithCameraDepth("ForegroundScene", 1);
    }
}