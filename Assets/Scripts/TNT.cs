using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : Unit {

    public Trap trap { get; set; }

    public int speed = 4;

    public void Move(Direction direction)
    {
        if (trap != null)
            trap.Move(direction);
        engine.AddToSnapshot(Clone());
        engine.RemovefromDatabase(this);
        Position = ToolKit.VectorSum(Position, direction);
        StartCoroutine(MoveCo(ToolKit.VectorSum(transform.position, direction)));
    }

    public bool CanMoveToPosition(Vector2 position, Direction direction)
    {
        if (!(position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY))
            return false;
        List<Unit> units = engine.units[(int)position.x, (int)position.y];
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Box || units[i] is Block || units[i] is Enemy || units[i] is TNT)
                return false;
            else if (units[i] is Trap)
            {
                if ((units[i] as Trap).CanMoveToPosition(ToolKit.VectorSum(Position, direction)))
                {
                    trap = units[i] as Trap;
                    return true;
                }
                return false;
            }
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
        engine.CheckSwitch();
        engine.AddtoDatabase(this);
    }

    public void Explode()
    {

    }

    public override Clonable Clone()
    {
        return new ClonableTNT(this);
    }

}

public class ClonableTNT : Clonable
{
    public ClonableTNT(TNT tnt)
    {
        original = tnt;
        position = tnt.Position;
        trasformposition = tnt.transform.position;
    }

    public override void Undo()
    {
        TNT tnt = original as TNT;
        tnt.StopAllCoroutines();
        tnt.engine.RemovefromDatabase(original);
        tnt.Position = position;
        tnt.transform.position = trasformposition;
        tnt.engine.AddtoDatabase(original);
    }
}
