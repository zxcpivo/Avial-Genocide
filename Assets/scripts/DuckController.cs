using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Add this to handle scene reloading

public class DuckController : MonoBehaviour
{
    public GameObject duckPrefab;       // Prefab for normal ducks
    public GameObject blackDuckPrefab;  // Prefab for black ducks
    public GameObject deathScreenPrefab;  // Prefab for the death screen
    public float spawnInterval = 1.5f;  // Interval at which ducks appear
    public float duckLifetime = 3.0f;   // Time before the duck disappears if not shot

    private bool isGameOver = false;    // Track if the game is over

    private void Start()
    {
        // Start spawning ducks at intervals
        StartCoroutine(SpawnDucks());
    }

    private void Update()
    {
        // If the game is over and the player presses space, restart the game
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    // Coroutine to handle duck spawning
    IEnumerator SpawnDucks()
    {
        while (!isGameOver)
        {
            SpawnDuck();  // Call the method to spawn a duck
            yield return new WaitForSeconds(spawnInterval);  // Wait for the next spawn interval
        }
    }

    // Method to spawn a duck at a random position
    private void SpawnDuck()
    {
        if (isGameOver) return;  // Don't spawn if the game is over

        Vector2 randomPosition = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));  // Random position within screen bounds

        // Randomly decide whether to spawn a normal duck or a black duck
        GameObject duckPrefabToSpawn = (Random.value > 0.9f) ? blackDuckPrefab : duckPrefab;  // 10% chance to spawn a black duck

        GameObject duck = Instantiate(duckPrefabToSpawn, randomPosition, Quaternion.identity);  // Create duck instance

        DuckBehavior duckBehavior = duck.GetComponent<DuckBehavior>();  // Get DuckBehavior component
        if (duckBehavior != null)
        {
            duckBehavior.SetDuckController(this);  // Pass reference of DuckController to DuckBehavior
            // If it's a black duck, mark it as such
            if (duckPrefabToSpawn == blackDuckPrefab)
            {
                duckBehavior.isBlackDuck = true;
            }
        }

        StartCoroutine(DuckLifetime(duck));  // Start the coroutine for duck lifetime
    }

    // Coroutine to handle duck lifetime
    IEnumerator DuckLifetime(GameObject duck)
    {
        yield return new WaitForSeconds(duckLifetime);  // Wait for the duck's lifetime

        // Check if the duck is still present before destroying it
        if (duck != null && !isGameOver)
        {
            Destroy(duck);  // Destroy duck if still present
            SpawnDuck();  // Immediately spawn a new duck
        }
    }

    // Method to handle duck destruction by clicking
    public void OnDuckDestroyed(bool isBlackDuck)
    {
        if (isBlackDuck)
        {
            GameOver();  // Trigger game over if a black duck was clicked
        }
        else
        {
            SpawnDuck();  // Spawn a new duck if it was a normal duck
        }
    }

    // Method to trigger game over
    private void GameOver()
    {
        isGameOver = true;  // Stop duck spawning

        Debug.Log("Game over triggered");

        // Instantiate the death screen prefab
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

        // Destroy all active ducks (optional: freeze instead of destroy if needed)
        DuckBehavior[] allDucks = FindObjectsOfType<DuckBehavior>();
        foreach (DuckBehavior duck in allDucks)
        {
            if (duck != null)
            {
                Destroy(duck.gameObject);  // Destroy all ducks to freeze them
                Debug.Log("Duck destroyed: " + duck.gameObject.name);
            }
        }
    }

    // Method to restart the game
    private void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}