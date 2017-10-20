using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    public GameObject menu;
    public GameObject levels;
    Engine engine;
    Starter starter;

    void Start()
    {
        try
        {
            starter = GameObject.Find("Starter").GetComponent<Starter>();
            engine = starter.engine;
        }
        catch { }
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Video");
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Undo()
    {
        GameObject.Find("Input").GetComponent<GetInput>().inMenuHide();
        engine.Undo();
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene("Menu");
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
