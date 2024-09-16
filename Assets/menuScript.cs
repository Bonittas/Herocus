using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string playerTag = "Player";
    public AudioClip gameSuccessClip; // Assign in inspector
    public AudioClip gameOverClip;    // Assign in inspector

    private AudioSource audioSource;

    private void Start()
    {
        // Initialize AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

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
        // Call method to play scene-specific sound after scene is loaded
        StartCoroutine(PlaySceneSound(sceneName));
    }

    private IEnumerator PlaySceneSound(string sceneName)
    {
        // Wait a frame to ensure scene is fully loaded
        yield return null;

        if (audioSource != null)
        {
            if (sceneName == "GameSuccess")
            {
                audioSource.clip = gameSuccessClip;
            }
            else if (sceneName == "GameOver")
            {
                audioSource.clip = gameOverClip;
            }

            // Play the sound if a valid clip is assigned
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("No AudioSource component found on this GameObject.");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
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
