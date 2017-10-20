using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour {

    public Vector2 Size;
    public Vector2 StartPos;
    public Engine engine;
	// Use this for initialization
	void Awake () {
        engine.turn = Turn.EnemyTurn;
        engine.sizeX = (int)Size.x;
        engine.sizeY = (int)Size.y;
        engine.enemies = new List<Enemy>();
        engine.traps = new List<Trap>();
        Init();
        engine.Begin();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void Init()
    {
        Init_Tiles();
        Init_Units();
        //engine.player.transform.position = StartPos;
    }

    private void Init_Units()
    {
        List<Unit>[,] units = new List<Unit>[engine.sizeX,engine.sizeY];
        for(int i=0; i<engine.sizeX; i++)
        {
            for(int j=0; j<engine.sizeY; j++)
            {
                units[i, j] = new List<Unit>();
            }
        }
        GameObject U = GameObject.Find("Units");
        for(int i=0; i<U.transform.childCount; i++)
        {
            Unit unit = U.transform.GetChild(i).GetComponent<Unit>();
            unit.Position = new Vector2((int)unit.transform.position.x, (int)unit.transform.position.y);
            units[(int)unit.Position.x, (int)unit.Position.y].Add(unit);
            unit.engine = engine;
            if(unit is Player)
            {
                engine.player = unit as Player;
                (unit as Player).prevpos = unit.Position;
            }
            else if(unit is Trap)
            {
                engine.traps.Add(unit as Trap);
            }
            else if(unit is End)
            {
                engine.endtile = unit as End; 
            }
            else if(unit is Key)
            {
                engine.key = unit as Key;
            }
            else if(unit is Switch)
            {
                engine.switchh = unit as Switch;
            }
            else if(unit is Enemy)
            {
                Debug.Log(unit);
                engine.enemies.Add(unit as Enemy);
                (unit as Enemy).PlayerPos = unit.Position;
            }
        }
        engine.units = units;
    }

    private void Init_Tiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Stone");
        System.Random rand = new System.Random();
        GameObject Tileparent = new GameObject("Tiles Parent");
        Tileparent.layer = 2;
        for(int i=0; i<engine.sizeX; i++)
        {
            for(int j=0; j<engine.sizeY; j++)
            {
                GameObject tile = Instantiate(tiles[rand.Next(0, tiles.Length)]);
                tile.transform.position = new Vector3(i,j,10);
                tile.transform.parent = Tileparent.transform;
            }
        }
    }
}
