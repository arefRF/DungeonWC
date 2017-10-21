using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Unit {

    public void Move(Direction direction)
    {
        engine.AddToSnapshot(Clone());
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
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Box || units[i] is Block || units[i] is Enemy || units[i] is TNT)
                return false;
        }
        return true;
    }

    public override Clonable Clone()
    {
        return new ClonableTrap(this);
    }
}

public class ClonableTrap : Clonable
{
    public ClonableTrap(Trap trap)
    {
        original = trap;
        position = trap.Position;
        trasformposition = trap.transform.position;
    }

    public override void Undo()
    {
        Trap trap = original as Trap;
        trap.engine.RemovefromDatabase(original);
        trap.Position = position;
        trap.transform.position = trasformposition;
        trap.engine.AddtoDatabase(original);
    }
}
