using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {

    public Key key;

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
            transform.position = ToolKit.VectorSum(transform.position, direction);
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
