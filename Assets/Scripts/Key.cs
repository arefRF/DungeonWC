using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Unit {

    public int speed = 4;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool CanMoveToPosition(Vector2 position)
    {
        if (!(position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY))
            return false;
        List<Unit> units = engine.units[(int)position.x, (int)position.y];
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Box || units[i] is Block || units[i] is Enemy || units[i] is TNT || units[i] is Trap)
                return false;
        }
        return true;
    }

    public void Move(Direction direction)
    {
        engine.AddToSnapshot(Clone());
        engine.RemovefromDatabase(this);
        Position = ToolKit.VectorSum(Position, direction);
        StartCoroutine(MoveCo(ToolKit.VectorSum(transform.position, direction)));
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
        return new ClonableKey(this);
    }
}

public class ClonableKey : Clonable
{
    public ClonableKey(Key key)
    {
        original = key;
        position = key.Position;
        trasformposition = key.transform.position;
    }

    public override void Undo()
    {
        Key key = original as Key;
        key.engine.RemovefromDatabase(original);
        key.Position = position;
        key.transform.position = trasformposition;
        key.engine.AddtoDatabase(original);
    }
}
