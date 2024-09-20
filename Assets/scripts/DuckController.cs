using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DuckController : MonoBehaviour
{
    public GameObject whiteDuckPrefab;
    public GameObject greenDuckPrefab;
    public GameObject yellowDuckPrefab;
    public GameObject goldenGoosePrefab;
    public GameObject blackDuckPrefab;  // The black (evil) duck prefab
    public GameObject deathScreenPrefab;
    public GameObject deathScreenTextPrefab;
    public GameObject pauseMenuPrefab;
    public Text healthText;
    public Text duckClickCounterText;
    public Text highScoreText;
    public RedFlashController redFlashController;
    public float spawnInterval = 1.5f;
    public float duckLifetime = 3.0f;

    private bool isGameOver = false;
    public bool isPaused = false;
    public GameObject pauseMenuInstance;

    private int health = 3;
    private int totalDucksClicked = 0;
    private int highScore = 0;
    private float blackDuckSpawnChance = 0.1f;  // Set a spawn chance for the black duck (10%)

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHealthUI();
        UpdateDuckClickCounterUI();
        UpdateHighScoreUI();
        StartCoroutine(SpawnDucks());
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }

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

        GameObject duckPrefabToSpawn = SelectDuckPrefab(); // Select the duck type based on rarity
        GameObject duck = Instantiate(duckPrefabToSpawn, randomPosition, Quaternion.identity);

        DuckBehavior duckBehavior = duck.GetComponent<DuckBehavior>();
        if (duckBehavior != null)
        {
            duckBehavior.SetDuckController(this);

            // Assign points based on the duck type
            if (duckPrefabToSpawn == whiteDuckPrefab)
                duckBehavior.points = 1;  // White duck
            else if (duckPrefabToSpawn == greenDuckPrefab)
                duckBehavior.points = 2;  // Green duck
            else if (duckPrefabToSpawn == yellowDuckPrefab)
                duckBehavior.points = 3;  // Yellow duck
            else if (duckPrefabToSpawn == goldenGoosePrefab)
                duckBehavior.points = 10;  // Golden goose

            // Mark as black duck if blackDuckPrefab
            if (duckPrefabToSpawn == blackDuckPrefab)
                duckBehavior.isBlackDuck = true;

            StartCoroutine(DuckLifetime(duck));
        }
    }

    // Select the duck type based on rarity
    private GameObject SelectDuckPrefab()
    {
        float randomValue = Random.value;

        // Check if the black duck should spawn (before other rare ducks)
        if (randomValue < blackDuckSpawnChance)
            return blackDuckPrefab;
        else if (randomValue < 0.05f)  // 5% chance for Golden Goose
            return goldenGoosePrefab;
        else if (randomValue < 0.15f)  // 10% chance for Yellow Duck
            return yellowDuckPrefab;
        else if (randomValue < 0.35f)  // 20% chance for Green Duck
            return greenDuckPrefab;
        else  // 65% chance for White Duck
            return whiteDuckPrefab;
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
    public void OnDuckDestroyed(bool isBlackDuck, int points)
    {
        if (isBlackDuck)
        {
            health--;  // Reduce health if black duck is clicked
            UpdateHealthUI();

            if (redFlashController != null)
            {
                redFlashController.TriggerRedFlash();
            }

            if (health <= 0)
            {
                GameOver();
            }
        }
        else
        {
            totalDucksClicked += points;  // Add points based on the duck type
            UpdateDuckClickCounterUI();
        }

        SpawnDuck();  // Continue spawning ducks after one is clicked
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health.ToString();
        }
    }

    private void UpdateDuckClickCounterUI()
    {
        if (duckClickCounterText != null)
        {
            duckClickCounterText.text = "Ducks Clicked: " + totalDucksClicked.ToString();
        }
    }

    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }

    private void GameOver()
    {
        isGameOver = true;

        if (totalDucksClicked > highScore)
        {
            highScore = totalDucksClicked;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateHighScoreUI();

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

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuInstance != null)
        {
            pauseMenuInstance.SetActive(true);
        }
    }

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
