using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuUI;    // Pause menu UI panel
    public Button pauseButton;        // Reference to the Pause button
    public Button resumeButton;       // Reference to the Resume button
    public AudioSource gameSound;     // Reference to game music/audio source

    private bool isPaused = false;

    private void Start()
    {
        // Initially, pause menu UI is hidden, Pause button is visible, and Resume button is hidden
        pauseMenuUI.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);

        // Add listeners to the buttons to handle click events
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;  // Freeze game time
            gameSound.Pause();    // Pause game music

            // Show the pause menu and Resume button, hide the Pause button
            pauseMenuUI.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            resumeButton.gameObject.SetActive(true);  // Show Resume button
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;  // Resume game time
            gameSound.Play();     // Resume game music

            // Hide the pause menu and Resume button, show the Pause button
            pauseMenuUI.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(false);  // Hide Resume button
        }
    }
}
