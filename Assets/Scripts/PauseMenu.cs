using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject Pausebtn;
    public GameObject Resumebtn;
    public GameObject MuteBtn;
    public GameObject Unmutebtn;
    public AudioMixer audioMixer;

    void Start()
    {
        MuteBtn.SetActive(!GameContext.IsGameMuted);
        Unmutebtn.SetActive(GameContext.IsGameMuted);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameContext.GameIsPaused)
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
        GameContext.GameIsPaused = false;
        Pausebtn.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameContext.GameIsPaused = true;
        Pausebtn.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menuscene");
        Resume();
    }

    public void Restart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene("NewStagesScenedup");
        Resume();
    }

    public void DeathMenu()
    {
        GameContext.sDeaths++;
        GameContext.Reset();
        Pause();
        Resumebtn.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MuteGame()
    {
        MuteBtn.SetActive(GameContext.IsGameMuted);
        Unmutebtn.SetActive(!GameContext.IsGameMuted);
        audioMixer.SetFloat("Volume", GameContext.IsGameMuted ? 0.0f : -80.0f);
        GameContext.IsGameMuted = !GameContext.IsGameMuted;
    }
}
