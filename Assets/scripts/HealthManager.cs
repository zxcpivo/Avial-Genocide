using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 3;          // Max health is 3
    private int currentHealth;         // Track current health

    public Text healthText;            // Reference to the health text UI
    public Text gameOverText;          // Reference to the "Game Over" UI text
    public Button damageButton;        // Reference to the button that reduces health

    void Start()
    {
        currentHealth = maxHealth;     // Set initial health to 3
        gameOverText.gameObject.SetActive(false);  // Hide "Game Over" text initially
        UpdateHealthUI();              // Initial update of health UI

        damageButton.onClick.AddListener(TakeDamage); // Add listener to the button
    }

    // This function is called every time the button is clicked
    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;           // Decrease health by 1
            UpdateHealthUI();          // Update UI with new health

            if (currentHealth <= 0)
            {
                GameOver();            // Call game over when health reaches 0
            }
        }
    }

    // Updates the health text UI
    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth.ToString(); // Update health UI
    }

    // Displays "Game Over" when health is zero
    void GameOver()
    {
        gameOverText.gameObject.SetActive(true);  // Show "Game Over" text
        damageButton.interactable = false;        // Disable the button
    }
}
