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

    public List<Trap> traps { get; set; }

    private int counter = 0;
    private List<Snapshot> snapshots;
    private Snapshot currentSnapshot;
    void Awake()
    {

    }
    public void Begin()
    {
        snapshots = new List<Snapshot>();
        currentSnapshot = new Snapshot();
        for (int i = 0; i < enemies.Count; i++)
            enemies[i].UpdatePlayerPos();
        CheckSwitch();
        turn = Turn.PlayerTurn;
    }

    public void ForceMove()
    {
        Debug.Log("force move");
        player.ForceMove();
        for(int i=0; i<enemies.Count; i++)
        {
            enemies[i].ForceMove();
        }
    }

    public void Move(Direction direciton)
    {
        if (turn == Turn.PlayerTurn && !player.isdead)
        {
            turn = Turn.EnemyTurn;
            player.Move(direciton);
        }
        //else
          //  ForceMove();
    }

    public void Checkkey()
    {
        if (key == null)
            return;
        List<Unit> list = units[(int)key.Position.x, (int)key.Position.y];
        for(int i=0; i<list.Count; i++)
        {
            if((list[i] is Enemy && !(list[i] as Enemy).isdead) || list[i] is Player)
            {
                key.GetComponent<SpriteRenderer>().enabled = false;
                key.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                return;
            }
        }
        key.GetComponent<SpriteRenderer>().enabled = true;
        key.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }
    public void PlayerMoveFinished(bool playermoved)
    {
        if (!playermoved)
        {
            Debug.Log("here");
            turn = Turn.PlayerTurn;
            return;
        }
        List<Unit> t = units[(int)player.Position.x, (int)player.Position.y];
        for(int i=0; i<t.Count; i++)
        {
            if(t[i] is Trap && !(t[i] as Trap).isdestroyed)
            {
                player.Die();
                (t[i] as Trap).Destroy();
            }
        }
        if (player.Position == endtile.Position)
        {
            if(player.key != null || !endtile.IsLocked)
            {
                GameObject.Find("Change").GetComponent<ChangeSence>().ChangeScene();
            }
        }
        else if(key != null && player.key == null)
        {
            if(player.Position == key.Position)
            {
                player.key = key;
            }
        }
        Checkkey();
        CheckSwitch();
        if (enemies.Count == 0)
            EnemyMoveFinished();
        else
            EnemyMove();
    }

    private void EnemyMove()
    {
        for(int i=0; i<enemies.Count; i++)
        {
            if(!enemies[i].isdead)
                enemies[i].SetNextPos();
        }
        for(int i=0; i<enemies.Count; i++)
        {
            if (!enemies[i].isdead)
                enemies[i].Move();
            else
            {
                EnemyMoveFinished();

            }
        }
    }
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
    public void EnemyMoveFinished()
    {
        counter++;
        if (enemies.Count <= counter)
        {
            for(int i=0; i<enemies.Count; i++)
            {
                if (enemies[i].isdead)
                    continue;
                if (enemies[i] is Enemy_Mage)
                {
                    enemies[i].UpdatePlayerPos();
                    if (enemies[i].PlayerPos == player.Position)
                    {
                        Vector2 temppos = player.Position - enemies[i].Position;
                        if (temppos.x == 0 || temppos.y == 0)
                        {
                            if (temppos.x == 0)
                            {
                                if (temppos.y > 0)
                                    (enemies[i] as Enemy_Mage).ChargeFireBall(Direction.Up);
                                else
                                    (enemies[i] as Enemy_Mage).ChargeFireBall(Direction.Down);
                            }
                            else if (temppos.y == 0)
                            {
                                if (temppos.x > 0)
                                    (enemies[i] as Enemy_Mage).ChargeFireBall(Direction.Right);
                                else
                                    (enemies[i] as Enemy_Mage).ChargeFireBall(Direction.Left);
                            }
                        }
                    }
                }
                enemies[i].UpdatePlayerPos();
                if(enemies[i].Position == player.Position)
                {
                    player.Die();
                }
                if (key != null && enemies[i].key == null)
                {
                    if (enemies[i].Position == key.Position)
                    {
                        enemies[i].key = key;
                    }
                }
                for (int j=0; j<traps.Count; j++)
                {
                    if(traps[j].Position == enemies[i].Position)
                    {
                        enemies[i].Die();
                        traps[j].Destroy();
                    }
                }
            }
            Checkkey();
            CheckSwitch();
            counter = 0;
            SnapshotDone();
            if (player.Position == endtile.Position)
            {
                if (player.key != null || !endtile.IsLocked)
                {
                    GameObject.Find("Change").GetComponent<ChangeSence>().ChangeScene();
                }
            }
            turn = Turn.PlayerTurn;
        }
    }

    public void CheckSwitch()
    {
        if(switchh != null && switchh.isOn)
        {
            List<Unit> temp = units[(int)switchh.Position.x, (int)switchh.Position.y];
            if (temp.Count == 1)
            {
                switchh.UnlockSound();
                endtile.Lock();
                switchh.isOn = false;
                switchh.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                switchh.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        if(switchh != null && !switchh.isOn)
        {
            
            List<Unit> temp = units[(int)switchh.Position.x, (int)switchh.Position.y];
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i] is Player || temp[i] is Box || temp[i] is Enemy)
                {
                    switchh.UnlockSound();
                    endtile.Unlock();
                    switchh.isOn = true;
                    switchh.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                    switchh.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }

    public void Undo()
    {
        if (!currentSnapshot.isempty)
            SnapshotDone();
        if (snapshots.Count == 0)
            return;
        Snapshot temp = snapshots[snapshots.Count - 1];
        snapshots.RemoveAt(snapshots.Count - 1);
        for(int i=0; i<temp.clones.Count; i++)
        {
            temp.clones[i].Undo();
        }
        Checkkey();
        CheckSwitch();
        turn = Turn.PlayerTurn;
    }

    public void AddToSnapshot(Clonable clone)
    {
        currentSnapshot.isempty = false;
        currentSnapshot.clones.Add(clone);
    }

    public void SnapshotDone()
    {
        snapshots.Add(currentSnapshot);
        currentSnapshot = new Snapshot();
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

public class Snapshot
{
    public List<Clonable> clones;
    public bool isempty;
    public Snapshot()
    {
        isempty = true;
        clones = new List<Clonable>();
    }
}
