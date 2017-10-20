using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Unit {

    public int speed = 4;
    public bool isdestroyed = false;
    public void Move(Direction direction)
    {
        engine.AddToSnapshot(Clone());
        engine.RemovefromDatabase(this);
        Position = ToolKit.VectorSum(Position, direction);
        StartCoroutine(MoveCo(ToolKit.VectorSum(transform.position, direction)));
    }

    public bool CanMoveToPosition(Vector2 position)
    {
        if (!(position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY))
            return false;
        List<Unit> units = engine.units[(int)position.x, (int)position.y];
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Box || units[i] is Block || units[i] is Enemy || units[i] is TNT || units[i] is Key)
                return false;
        }
        return true;
    }
    private IEnumerator MoveCo(Vector3 nextPos)
    {
        float remain = (transform.position - nextPos).sqrMagnitude;
        while (remain > float.Epsilon)
        {
            remain = (transform.position - nextPos).sqrMagnitude;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * speed);
            yield return null;
        }

        /// Move Finished
        engine.AddtoDatabase(this);
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
        trap.StopAllCoroutines();
        trap.engine.RemovefromDatabase(original);
        trap.Position = position;
        trap.transform.position = trasformposition;
        trap.engine.AddtoDatabase(original);
    }
}
