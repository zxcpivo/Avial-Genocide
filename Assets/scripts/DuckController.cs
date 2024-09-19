using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DuckController : MonoBehaviour
{
    public GameObject duckPrefab;
    public GameObject blackDuckPrefab;
    public GameObject deathScreenPrefab;
    public GameObject deathScreenTextPrefab;  // Add this field for the text prefab
    public float spawnInterval = 1.5f;
    public float duckLifetime = 3.0f;

    private bool isGameOver = false;

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
    }

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

        Debug.Log("Game over triggered");

        if (deathScreenPrefab != null)
        {
            GameObject deathScreen = Instantiate(deathScreenPrefab);
            deathScreen.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            Debug.Log("Death screen instantiated and positioned.");
        }
        else
        {
            Debug.LogError("Death screen prefab is not assigned in the Inspector!");
        }

        // Instantiate the text to show "Press 'Space' to try again"
        if (deathScreenTextPrefab != null)
        {
            GameObject deathScreenText = Instantiate(deathScreenTextPrefab);
            deathScreenText.transform.SetParent(GameObject.Find("Canvas").transform, false);  // Attach the text to the Canvas
            Debug.Log("Death screen text instantiated.");
        }
        else
        {
            Debug.LogError("Death screen text prefab is not assigned in the Inspector!");
        }

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