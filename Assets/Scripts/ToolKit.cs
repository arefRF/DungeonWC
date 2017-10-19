using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolKit{

	public static Vector2 VectorSum(Vector2 pos, Direction dir)
    {
        switch (dir)
        {
            case Direction.Down: return new Vector2(pos.x, pos.y - 1);
            case Direction.Left: return new Vector2(pos.x - 1, pos.y);
            case Direction.Right: return new Vector2(pos.x + 1, pos.y);
            case Direction.Up: return new Vector2(pos.x, pos.y + 1);
        }
        return new Vector2(0,0);
    }

    public static Direction IntToDirection(int i)
    {
        switch (i)
        {
            case 0: return Direction.Up;
            case 1: return Direction.Right;
            case 2: return Direction.Down;
            case 3: return Direction.Left;
        }
        return Direction.Right;
    }
}
