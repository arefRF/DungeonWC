using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Unit {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
