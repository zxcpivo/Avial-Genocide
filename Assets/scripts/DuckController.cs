using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class DuckController : MonoBehaviour
{
    public GameObject duckPrefab;       
    public GameObject blackDuckPrefab;  
    public GameObject deathScreenPrefab; 
    public float spawnInterval = 1.5f;  
    public float duckLifetime = 3.0f;   

    private bool isGameOver = false;    // Track if the game is over

    private void Start()
    {
        StartCoroutine(SpawnDucks());
    }

    private void Update()
    {
        // if the game is over and the player presses space this restarts the game
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    // coroutine for duck spawning
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

        StartCoroutine(DuckLifetime(duck));  //coroutine for duck lifetime
    }

    IEnumerator DuckLifetime(GameObject duck)
    {
        yield return new WaitForSeconds(duckLifetime);

        // check if the duck is still present before destroying it
        if (duck != null && !isGameOver)
        {
            Destroy(duck);  
            SpawnDuck();  
        }
    }

    // duck destruction by clicking
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

        Debug.Log("Game over triggered");

        //death screen prefab
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}