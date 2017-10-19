using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Unit {

    public void Move(Direction direction)
    {
        engine.RemovefromDatabase(this);
        Position = ToolKit.VectorSum(Position, direction);
        transform.position = ToolKit.VectorSum(transform.position, direction);
        engine.AddtoDatabase(this);
    }

	public bool CanMoveToPosition(Vector2 position)
    {
        if (position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY)
            return true;
        return false;
    }
}
