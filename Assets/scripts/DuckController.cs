using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    public GameObject duckPrefab;  // Prefab for the duck sprite
    public float spawnInterval = 1.5f;  // Interval at which ducks appear
    public float duckLifetime = 3.0f;  // Time before the duck disappears if not shot

    private void Start()
    {
        // Start spawning ducks at intervals
        StartCoroutine(SpawnDucks());
    }

    // Coroutine to handle duck spawning
    IEnumerator SpawnDucks()
    {
        while (true)
        {
            SpawnDuck();  // Call the method to spawn a duck
            yield return new WaitForSeconds(spawnInterval);  // Wait for the next spawn interval
        }
    }

    // Method to spawn a duck at a random position
    private void SpawnDuck()
    {
        Vector2 randomPosition = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));  // Random position within screen bounds
        GameObject duck = Instantiate(duckPrefab, randomPosition, Quaternion.identity);  // Create duck instance

        DuckBehavior duckBehavior = duck.GetComponent<DuckBehavior>();  // Get DuckBehavior component
        if (duckBehavior != null)
        {
            duckBehavior.SetDuckController(this);  // Pass reference of DuckController to DuckBehavior
        }

        StartCoroutine(DuckLifetime(duck));  // Start the coroutine for duck lifetime
    }

    // Coroutine to handle duck lifetime
    IEnumerator DuckLifetime(GameObject duck)
    {
        yield return new WaitForSeconds(duckLifetime);  // Wait for the duck's lifetime

        // Check if the duck is still present before destroying it
        if (duck != null)
        {
            Destroy(duck);  // Destroy duck if still present
            SpawnDuck();  // Immediately spawn a new duck
        }
    }

    // Method to handle duck destruction by clicking
    public void OnDuckDestroyed()
    {
        SpawnDuck();  // Spawn a new duck when one is destroyed
    }
}