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
        if (!(position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY))
            return false;
        List<Unit> units = engine.units[(int)position.x, (int)position.y];
        for(int i=0; i<units.Count; i++)
        {
            if (units[i] is Box || units[i] is Block || units[i] is Enemy)
                return false;
        }
        return true;
    }
}
