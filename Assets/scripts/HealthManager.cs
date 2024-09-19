using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 3;         
    private int currentHealth;         

    public Text healthText;            
    public Text gameOverText;          
    public Button damageButton;       

    void Start()
    {
        currentHealth = maxHealth;     
        gameOverText.gameObject.SetActive(false);  
        UpdateHealthUI();             

        damageButton.onClick.AddListener(TakeDamage); 
    }


    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;          
            UpdateHealthUI();          

            if (currentHealth <= 0)
            {
                GameOver();            
            }
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth.ToString(); 
    }

    // Displays "Game Over" when health is zero
    void GameOver()
    {
        gameOverText.gameObject.SetActive(true);  
        damageButton.interactable = false;        
    }
}
