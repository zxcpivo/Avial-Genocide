using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckBehavior : MonoBehaviour
{
    private DuckController duckController;  // Reference to DuckController
    public bool isBlackDuck = false;  // A flag to check if this is the black duck
    public int points = 1;  // Default points for ducks, will be overridden based on duck type

    public void SetDuckController(DuckController controller)
    {
        duckController = controller;  // Set the reference to DuckController
    }

    private void OnMouseDown()
    {
        // Check if the game is paused or if duckController is null
        if (duckController == null || duckController.isPaused)
        {
            return;  // Do nothing if the game is paused or duckController is not set
        }

        // Notify DuckController that this duck was destroyed and pass whether it's a black duck and points
        if (duckController != null)
        {
            duckController.OnDuckDestroyed(isBlackDuck, points);  // Pass both black duck status and points
        }

        // Destroy the duck when clicked
        Destroy(gameObject);

        // Log the type of duck that was clicked
        Debug.Log(isBlackDuck ? "Black Duck Clicked!" : "Normal Duck Shot!");
    }
}
