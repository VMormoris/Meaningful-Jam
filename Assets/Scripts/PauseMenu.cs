using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject Pausebtn;
    public GameObject Resumebtn;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Pausebtn.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menuscene");
    }

    public void Restart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene("NewStagesScenedup");
        Resume();
    }

    public void DeathMenu()
    {
        Pause();
        Resumebtn.SetActive(false);
        Time.timeScale = 1f;
    }
}
