using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    public GameObject menu;
    public GameObject levels;
    public void NewGame()
    {
        SceneManager.LoadScene("Intro");
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Levels()
    {
        menu.SetActive(false);
        levels.SetActive(true);
    }

    public void Back()
    {
        menu.SetActive(true);
        levels.SetActive(false);
    }
}
