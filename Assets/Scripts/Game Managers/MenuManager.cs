using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager: MonoBehaviour
{

    // MENU SCREENS
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject helpMenu;
    [SerializeField] GameObject settingsMenu;

    bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        helpMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    void Update()
    {
        CheckPause();
       
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        helpMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void GoToHelpMenu()
    {
        pauseMenu.SetActive(false);
        helpMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void GoToSettingsMenu()
    {
        pauseMenu.SetActive(false);
        helpMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
