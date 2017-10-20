using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInput : MonoBehaviour {

    Starter starter;
    Engine engine;
    // Use this for initialization
    void Start()
    {
        starter = GameObject.Find("Starter").GetComponent<Starter>();
        engine = starter.engine;
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
        if (Input.GetKeyUp(KeyCode.DownArrow))
            engine.Move(Direction.Down);
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            engine.Move(Direction.Left);
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            engine.Move(Direction.Right);
        else if (Input.GetKeyUp(KeyCode.UpArrow))
            engine.Move(Direction.Up);
        else if (Input.GetKeyUp(KeyCode.R))
            engine.Undo();
        else if (Input.GetKeyUp(KeyCode.Space))
            engine.Action();
    }
}
