using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInput : MonoBehaviour {

    Starter starter;
    Engine engine;
    public GameObject inMenu;
    // Use this for initialization
    void Start()
    {
        starter = GameObject.Find("Starter").GetComponent<Starter>();
        engine = starter.engine;
    }

    public void inMenuShow()
    {
        inMenu.SetActive(true);
    }

    public void inMenuHide()
    {
        inMenu.SetActive(false);
    }

    public void inMenuTrigger()
    {
        inMenu.SetActive(!inMenu.activeSelf);
    }
    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.RightArrow))
            engine.player.FakeMove(Direction.Right);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            engine.player.FakeMove(Direction.Left);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            engine.player.FakeMove(Direction.Up);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            engine.player.FakeMove(Direction.Down);
        */
        if (Input.GetKeyDown(KeyCode.DownArrow))
            engine.Move(Direction.Down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            engine.Move(Direction.Left);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            engine.Move(Direction.Right);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            engine.Move(Direction.Up);
        else if (Input.GetKeyDown(KeyCode.R))
            engine.Undo();
        else if (Input.GetKeyDown(KeyCode.Space))
            engine.Action();
        else if (Input.GetKeyDown(KeyCode.Escape))
            inMenuTrigger();
    }
}
