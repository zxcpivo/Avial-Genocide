using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckBehavior : MonoBehaviour
{
    private DuckController duckController;  // Reference to DuckController

    public void SetDuckController(DuckController controller)
    {
        duckController = controller;  // Set the reference to DuckController
    }

    private void OnMouseDown()
    {
        // Notify DuckController that this duck was destroyed
        if (duckController != null)
        {
            duckController.OnDuckDestroyed();
        }

        Destroy(gameObject);  // Destroy the duck when clicked
        Debug.Log("Duck Shot!");  // Print message for testing
    }
}