using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro namespace

public class MenuManager : MonoBehaviour
{
    public string playerTag = "Player";
    public AudioClip gameSuccessClip; // Assign in inspector
    public AudioClip gameOverClip;    // Assign in inspector
    public TextMeshProUGUI coinText;  // Change to TextMeshProUGUI

    private AudioSource audioSource;

    private void Start()
    {
        // Initialize AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Display the total coins if on GameOver or GameSuccess scene
        if (SceneManager.GetActiveScene().name == "GameOver" || SceneManager.GetActiveScene().name == "GameSuccess")
        {
            DisplayTotalCoins();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            StartCoroutine(LoadSuccessScene());
        }
    }

    private IEnumerator LoadSuccessScene()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        // Save the total coins using PlayerPrefs
        PlayerPrefs.SetInt("TotalCoins", FindObjectOfType<ClearSky.DemoCollegeStudentController>().GetTotalCoins());
        LoadScene("GameSuccess");
    }
        private IEnumerator LoadSuccessScene2()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        // Save the total coins using PlayerPrefs
        PlayerPrefs.SetInt("TotalCoins", FindObjectOfType<ClearSky.DemoCollegeStudentController>().GetTotalCoins());
        LoadScene("GameSuccess");
    }


    public void StartGame()
    {
        LoadScene("StartGame");
    }

    public void LoadGameOver()
    {
        // Save the total coins using PlayerPrefs
        PlayerPrefs.SetInt("TotalCoins", FindObjectOfType<ClearSky.DemoCollegeStudentController>().GetTotalCoins());
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
        public void LoadLevel3()
    {
        string nextLevelSceneNam = "Level3"; // Replace with your logic
        LoadScene(nextLevelSceneNam);
    }

    public void RestartGame()
    {
        if (SceneManager.GetSceneByName("GameOver").isLoaded)
        {
            SceneManager.UnloadSceneAsync("GameOver");
        }
        LoadScene("StartGame");
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        StartCoroutine(PlaySceneSound(sceneName));
    }

    private IEnumerator PlaySceneSound(string sceneName)
    {
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

            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Method to display total coins in Game Over or Success scenes
    private void DisplayTotalCoins()
    {
        if (coinText != null)
        {
            int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0); // Default to 0 if not found
            coinText.text = "" + totalCoins;
        }
        else
        {
            Debug.LogWarning("CoinText UI is not assigned.");
        }
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
