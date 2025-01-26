using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private List<GameObject> otherCanvases;

    
    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeSelf)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf)
        {
            Resume();
        }
    }
    
    public void Pause()
    {
        if (!InventoryManager.instance.menuActivated)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            foreach (var canvas in otherCanvases)
            {
                canvas.SetActive(false);
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        foreach (var canvas in otherCanvases)
        {
            canvas.SetActive(true);
        }
    }

    public void Quit()
    {
        // Jeśli w edytorze Unity, zakończ tryb gry
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Jeśli gra jest zbudowana, zamknij aplikację
        Application.Quit();
        #endif
    }
    
    public void OpenOptionsMenu()
    {
        settingsMenu.SetActive(true);
        //pauseMenu.SetActive(false);
    }
}
