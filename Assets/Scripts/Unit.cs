using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public Engine engine { get; set; }
    public Vector2 Position { get; set; }

    public Vector3 npos { get; set; }
    public virtual Clonable Clone()
    {
        return null;
    }
}

public class Clonable
{
    public Vector2 position;
    public Unit original;
    public Vector3 trasformposition;
    public virtual void Undo()
    {
    }
}
