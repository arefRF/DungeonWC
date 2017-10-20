using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {

    public Key key { get; set; }
    public Box box { get; set; }
    private Animator animator;
    private AnimationEventPlayer a_event;
    public Vector2 prevpos { get; set; }

    void Start()
    {
        a_event = GetComponentInChildren<AnimationEventPlayer>();
        animator = GetComponentInChildren<Animator>();
    }
    
    public void Move(Direction direction)
    {
        engine.RemovefromDatabase(this);
        Vector2 temppos = ToolKit.VectorSum(Position, direction);
        bool playermoved = false;
        if (CanMoveToPosition(temppos, direction))
        {
            engine.AddToSnapshot(Clone());
            prevpos = Position;
            Position = temppos;
            animator.SetInteger("Walk", 0);
            if (key != null)
            {
                engine.AddToSnapshot(key.Clone());
                engine.RemovefromDatabase(key);
                key.Position = ToolKit.VectorSum(key.Position, direction);
                key.transform.position = ToolKit.VectorSum(key.transform.position, direction);
                engine.AddtoDatabase(key);
            }
            if(box != null)
            {
                box.Move(direction);
                box = null;
            }
            playermoved = true;
        }
        engine.AddtoDatabase(this);
        MoveFinished(playermoved);
    }

    public void MoveFinished(bool playermoved)
    {
        engine.PlayerMoveFinieshed(playermoved);
    }


    public void FakeMove(Direction dir)
    {
        if (engine.turn == Turn.EnemyTurn)
            return;
        a_event.dir = dir;
        if (dir == Direction.Right)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("Walk", 1);
        }
        else if (dir == Direction.Left)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
            animator.SetInteger("Walk", 1);
        }

        else if (dir == Direction.Up)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("Walk", 2);
        }
        else if(dir == Direction.Down)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("Walk", 3);
        }
    }
    private bool CanMoveToPosition(Vector2 position, Direction direction)
    {
        if (!(position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY))
            return false;
        List<Unit> units = engine.units[(int)position.x, (int)position.y];
        for(int i=0; i<units.Count; i++)
        {
            if(units[i] is Box)
            {
                if ((units[i] as Box).CanMoveToPosition(ToolKit.VectorSum(position, direction)))
                {
                    box = units[i] as Box;
                    return true;
                }
                else
                    return false;
            }
            else if (units[i] is Enemy)
            {
                return false;
            }
        }
        return true;
    }

    public override Clonable Clone()
    {
        return new ClonablePlayer(this);
    }
}

public class ClonablePlayer: Clonable
{
    public Key key;
    public Vector2 prevpos;


    public ClonablePlayer(Player player)
    {
        original = player;
        position = player.Position;
        key = player.key;
        prevpos = player.prevpos;
        trasformposition = player.transform.position;
    }

    public override void Undo()
    {
        Player player = original as Player;
        player.engine.RemovefromDatabase(original);
        player.Position = position;
        player.key = key;
        player.prevpos = prevpos;
        player.transform.position = trasformposition;
        player.engine.AddtoDatabase(original);
    }
}
