using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this to use UI elements
using UnityEngine.SceneManagement;

public class DuckController : MonoBehaviour
{
    public GameObject duckPrefab;
    public GameObject blackDuckPrefab;
    public GameObject deathScreenPrefab;
    public Text healthText;  // UI Text to display health
    public float spawnInterval = 1.5f;
    public float duckLifetime = 3.0f;

    private int health = 3;  // Player's health starts at 3
    private bool isGameOver = false;    // Track if the game is over

    private void Start()
    {
        UpdateHealthText();  // Update UI when game starts
        StartCoroutine(SpawnDucks());
    }

    private void Update()
    {
        // If the game is over and the player presses space, this restarts the game
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    // Coroutine for duck spawning
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

        Vector2 randomPosition = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));  // Random position within screen bounds

        // Randomly decide whether to spawn a normal duck or a black duck
        GameObject duckPrefabToSpawn = (Random.value > 0.9f) ? blackDuckPrefab : duckPrefab;  // 10% chance to spawn a black duck

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

        StartCoroutine(DuckLifetime(duck));  // Coroutine for duck lifetime
    }

    IEnumerator DuckLifetime(GameObject duck)
    {
        yield return new WaitForSeconds(duckLifetime);

        // Check if the duck is still present before destroying it
        if (duck != null && !isGameOver)
        {
            Destroy(duck);
            SpawnDuck();
        }
    }

    // Duck destruction by clicking
    public void OnDuckDestroyed(bool isBlackDuck)
    {
        if (isBlackDuck)
        {
            LoseHealth();  // Lose health when black duck is clicked
        }
        else
        {
            SpawnDuck();  // Spawn another duck when a normal duck is destroyed
        }
    }

    private void LoseHealth()
    {
        if (isGameOver) return;

        health--;  // Reduce health by 1
        UpdateHealthText();  // Update the health text on the UI

        if (health <= 0)
        {
            GameOver();  // Trigger game over when health reaches 0
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health;  // Update the text element with current health
        }
    }

    private void GameOver()
    {
        isGameOver = true;  // Set game over flag

        Debug.Log("Game over triggered");

        // Death screen prefab
        if (deathScreenPrefab != null)
        {
            GameObject deathScreen = Instantiate(deathScreenPrefab);
            deathScreen.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);  // Center the death screen
            Debug.Log("Death screen instantiated and positioned.");
        }
        else
        {
            Debug.LogError("Death screen prefab is not assigned in the Inspector!");
        }

        // Destroy all active ducks
        DuckBehavior[] allDucks = FindObjectsOfType<DuckBehavior>();
        foreach (DuckBehavior duck in allDucks)
        {
            if (duck != null)
            {
                Destroy(duck.gameObject);
                Debug.Log("Duck destroyed: " + duck.gameObject.name);
            }
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Restart the scene
    }
}
