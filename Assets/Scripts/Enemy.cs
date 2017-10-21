using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

    public Vector2 NextPos { get; set; }
    public Key key { get; set; }
    public Vector2 PlayerPos { get; set; }
    public Vector2 PlayerPostemp { get; set; }
    public virtual void SetNextPos()
    {
       
    }

	public virtual void Move()
    {
         
    }

    public virtual bool CanMoveToPosition(Vector2 position)
    {
        if(!(position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY))
            return false;
        List<Unit> units = engine.units[(int)position.x, (int)position.y];
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Box || units[i] is Block)
                return false;
        }
        return true;
    }

    public void UpdatePlayerPos()
    {
        PlayerPostemp = PlayerPos;
        RaycastHit2D hit = Physics2D.Raycast(Position, (engine.player.Position - Position).normalized);
        if (hit.collider != null)
        {
            Player player = hit.collider.gameObject.GetComponent<Player>();
            if(player != null)
            {
                PlayerPos = player.Position;
            }
        }
    }
}
