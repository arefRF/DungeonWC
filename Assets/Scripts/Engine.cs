using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {

    public Turn turn { get; set; }
    public int sizeX { get; set; }
    public int sizeY { get; set; }
    public Player player { get; set; }
    public End endtile { get; set; }
    public Key key { get; set; }

    public Switch switchh { get; set; }
    public List<Unit>[,] units {get; set; }
    public List<Enemy> enemies { get; set; }

    private int counter = 0;
    void Awake()
    {

    }

    public void Begin()
    {
        for (int i = 0; i < enemies.Count; i++)
            enemies[i].UpdatePlayerPos();
        turn = Turn.PlayerTurn;
    }

    public void Move(Direction direciton)
    {
        if(turn == Turn.PlayerTurn)
        {
            turn = Turn.EnemyTurn;
            player.Move(direciton);
        }
    }

    public void PlayerMoveFinieshed(bool playermoved)
    {
        if (!playermoved)
        {
            turn = Turn.PlayerTurn;
            return;
        }
        if (player.Position == endtile.Position)
        {
            if(player.key != null || !endtile.IsLocked)
            {
                Debug.Log("Level Finished");
            }
        }
        else if(key != null && player.key == null)
        {
            if(player.Position == key.Position)
            {
                player.key = key;
            }
        }
        CheckSwitch();
        EnemyMove();
    }

    private void EnemyMove()
    {
        for(int i=0; i<enemies.Count; i++)
        {
            enemies[i].SetNextPos();
        }
        for(int i=0; i<enemies.Count; i++)
        {
            enemies[i].Move();
        }
    }

    public void EnemyMoveFinished()
    {
        counter++;
        if (enemies.Count == counter)
        {
            counter = 0;
            turn = Turn.PlayerTurn;
        }
    }

    public void CheckSwitch()
    {
        if(switchh != null && !switchh.isOn)
        {
            List<Unit> temp = units[(int)switchh.Position.x, (int)switchh.Position.y];
            if (temp.Count == 1)
            {
                endtile.Lock();
                switchh.isOn = false;
            }
            else
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i] is Player || temp[i] is Box || temp[i] is Enemy)
                    {
                        endtile.Unlock();
                    }
                }
            }
        }
    }

    public void Undo()
    {
        Debug.Log("undo");
    }

    public void Action()
    {
        Debug.Log("action");
    }

    public void AddtoDatabase(Unit unit)
    {
        units[(int)unit.Position.x, (int)unit.Position.y].Add(unit);
    }

    public void RemovefromDatabase(Unit unit)
    {
        units[(int)unit.Position.x, (int)unit.Position.y].Remove(unit);
    }
}

public enum Direction
{
    Up, Right, Down, Left
}

public enum Turn
{
    PlayerTurn, EnemyTurn
}
