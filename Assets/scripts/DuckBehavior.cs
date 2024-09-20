using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckBehavior : MonoBehaviour
{
    private DuckController duckController;  // Reference to DuckController
    public bool isBlackDuck = false;        // A flag to check if this is the black duck

    public void SetDuckController(DuckController controller)
    {
        duckController = controller;        // Set the reference to DuckController
    }

    private void OnMouseDown()
    {
        // Only allow duck to be clicked if the game is NOT paused
        if (Time.timeScale == 0f) return;

        // Notify DuckController that this duck was destroyed and pass whether it's a black duck
        if (duckController != null)
        {
            duckController.OnDuckDestroyed(isBlackDuck);
        }

        Destroy(gameObject);  // Destroy the duck when clicked
        Debug.Log(isBlackDuck ? "Black Duck Clicked!" : "Normal Duck Shot!");  // Log based on the duck type
    }
}
