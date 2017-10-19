using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {
    private Animator animator;
    public Key key;
    private AnimationEventPlayer a_event;

    void Start()
    {
        a_event = GetComponentInChildren<AnimationEventPlayer>();
        animator = GetComponentInChildren<Animator>();
    }
    public Vector2 prevpos { get; set; }
    public void Move(Direction direction)
    {
        engine.RemovefromDatabase(this);
        Vector2 temppos = ToolKit.VectorSum(Position, direction);
        bool playermoved = false;
        if (CanMoveToPosition(temppos, direction))
        {
            prevpos = Position;
            Position = temppos;
            animator.SetInteger("Walk", 0);
            if(key != null)
                key.transform.position = ToolKit.VectorSum(key.transform.position, direction);
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
                    (units[i] as Box).Move(direction);
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
}
