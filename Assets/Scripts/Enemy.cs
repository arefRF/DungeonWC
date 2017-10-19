using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

    public Vector2 NextPos { get; set; }

    public Vector2 PlayerPos { get; set; }

    public virtual void SetNextPos()
    {
       
    }

	public virtual void Move()
    {
         
    }

    public virtual bool CanMoveToPosition(Vector2 position)
    {
        return true;
    }

    public void UpdatePlayerPos()
    {
        PlayerPos = engine.player.Position;
    }
}
