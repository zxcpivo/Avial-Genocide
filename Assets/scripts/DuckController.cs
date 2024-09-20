using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DuckController : MonoBehaviour
{
    public GameObject duckPrefab;
    public GameObject blackDuckPrefab;
    public GameObject deathScreenPrefab;
    public GameObject deathScreenTextPrefab;
    public GameObject pauseMenuPrefab;
    public Text healthText;  // UI Text for health
    public float spawnInterval = 1.5f;
    public float duckLifetime = 3.0f;

    private bool isGameOver = false;
    public bool isPaused = false;
    public GameObject pauseMenuInstance;

    private int health = 3;  // Player starts with 3 health
    private float blackDuckSpawnChance = 0.3f;  // Increase spawn chance to 30%

    private void Start()
    {
        UpdateHealthUI();  // Initialize the health display
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
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
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
        if (isGameOver || isPaused) return;

        Vector2 randomPosition = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));

        // Increase the chance of spawning black ducks by lowering the threshold
        GameObject duckPrefabToSpawn = (Random.value < blackDuckSpawnChance) ? blackDuckPrefab : duckPrefab;
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

    // Call this when a duck is clicked
    public void OnDuckDestroyed(bool isBlackDuck)
    {
        if (isBlackDuck)
        {
            health--;  // Reduce health if black duck is clicked
            UpdateHealthUI();  // Update the health display

            if (health <= 0)
            {
                GameOver();  // Trigger game over if health reaches 0
            }
        }
        else
        {
            SpawnDuck();  // Continue spawning ducks if it's not a black duck
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health.ToString();
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
        Time.timeScale = 0f;

        if (pauseMenuInstance != null)
        {
            pauseMenuInstance.SetActive(true);
        }
    }

    // Method to resume the game
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuInstance != null)
        {
            pauseMenuInstance.SetActive(false);
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
