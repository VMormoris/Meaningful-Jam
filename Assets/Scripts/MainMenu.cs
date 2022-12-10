using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Load next scene from Scenes in build
    }

    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0); //loads first scene(MenuScene)
    }
    public void Quitgame()
    {
        Application.Quit();
    }
    
}
