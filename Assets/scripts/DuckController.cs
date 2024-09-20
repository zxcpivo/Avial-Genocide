using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DuckController : MonoBehaviour
{
    public GameObject duckPrefab;
    public GameObject blackDuckPrefab;
    public GameObject deathScreenPrefab;
    public GameObject deathScreenTextPrefab;
    public GameObject pauseMenuPrefab;  // Pause menu prefab
    public float spawnInterval = 1.5f;
    public float duckLifetime = 3.0f;

    private bool isGameOver = false;
    public bool isPaused = false;
    public GameObject pauseMenuInstance;  // To hold the instantiated pause menu

    private void Start()
    {
        StartCoroutine(SpawnDucks());
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }

        // Check for the Escape key to toggle the pause menu
        if (!isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(isPaused);
            if (isPaused)
            {
                ResumeGame();  // Resume the game if it was paused
            }
            else
            {
                PauseGame();  // Pause the game if it's not already paused
            }
        }
    }

    // Coroutine to handle duck spawning
    IEnumerator SpawnDucks()
    {
        while (!isGameOver)
        {
            SpawnDuck();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnDuck()
    {
        if (isGameOver) return;
        if (isPaused) return;

        Vector2 randomPosition = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
        GameObject duckPrefabToSpawn = (Random.value > 0.9f) ? blackDuckPrefab : duckPrefab;
        GameObject duck = Instantiate(duckPrefabToSpawn, randomPosition, Quaternion.identity);

        DuckBehavior duckBehavior = duck.GetComponent<DuckBehavior>();
        if (duckBehavior != null)
        {
            duckBehavior.SetDuckController(this);

            if (duckPrefabToSpawn == blackDuckPrefab)
            {
                duckBehavior.isBlackDuck = true;
            }
        }

        StartCoroutine(DuckLifetime(duck));
    }

    IEnumerator DuckLifetime(GameObject duck)
    {
        yield return new WaitForSeconds(duckLifetime);

        if (duck != null && !isGameOver)
        {
            Destroy(duck);
            SpawnDuck();
        }
    }

    public void OnDuckDestroyed(bool isBlackDuck)
    {
        if (isBlackDuck)
        {
            GameOver();
        }
        else
        {
            SpawnDuck();
        }
    }

    private void GameOver()
    {
        isGameOver = true;

        if (deathScreenPrefab != null)
        {
            GameObject deathScreen = Instantiate(deathScreenPrefab);
            deathScreen.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        }

        if (deathScreenTextPrefab != null)
        {
            GameObject deathScreenText = Instantiate(deathScreenTextPrefab);
            deathScreenText.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }

        DuckBehavior[] allDucks = FindObjectsOfType<DuckBehavior>();
        foreach (DuckBehavior duck in allDucks)
        {
            if (duck != null)
            {
                Destroy(duck.gameObject);
            }
        }
    }

    // Method to pause the game
    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Freeze the game

        // Instantiate the pause menu prefab
        if (pauseMenuInstance != null)
        {
            pauseMenuInstance.SetActive(true);
            // pauseMenuInstance = Instantiate(pauseMenuPrefab);
            // pauseMenuInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
    }

    // Method to resume the game
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Resume the game

        // Destroy the pause menu instance
        if (pauseMenuInstance != null)
        {
            pauseMenuInstance.SetActive(false);
            // Destroy(pauseMenuInstance);
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;  // Make sure to reset time scale when restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}