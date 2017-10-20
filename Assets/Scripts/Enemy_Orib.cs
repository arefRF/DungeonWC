using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Orib : Enemy {

    public override void SetNextPos()
    {
        UpdatePlayerPos();
        if (PlayerPos == Position)
        {
            NextPos = Position;
            return;
        }
        List<int> selected = new List<int>();
        Vector2[] poses = new Vector2[8];
        for(int i=0; i<4; i++)
        {
            poses[i] = ToolKit.VectorSum(Position, ToolKit.IntToDirection(i));
        }
        poses[4] = ToolKit.VectorSum(poses[0], Direction.Right);
        poses[5] = ToolKit.VectorSum(poses[1], Direction.Down);
        poses[6] = ToolKit.VectorSum(poses[2], Direction.Left);
        poses[7] = ToolKit.VectorSum(poses[3], Direction.Up);
        float min = 10000;
        for (int i = 0; i < 8; i++)
        {
            Vector2 temppos = poses[i];
            if (!CanMoveToPosition(temppos))
                continue;
            float temp = Vector2.SqrMagnitude(temppos - PlayerPos);
            if (temp < min && min - temp > 0.01)
            {
                selected.Clear();
                selected.Add(i);
                min = temp;
            }
            else if (min - temp < 0.01 && min >= temp)
            {
                selected.Add(i);
            }
        }
        Debug.Log(selected.Count);
        if (selected.Count == 0)
            engine.EnemyMoveFinished();
        if (selected.Count == 1)
        {
            NextPos = poses[selected[0]];
        }
        else
        {
            int sel = selected[0];
            if (PlayerPos == engine.player.Position)
            {
                min = 1000;
                for (int i = 0; i < selected.Count; i++)
                {
                    Vector2 temppos = ToolKit.VectorSum(Position, ToolKit.IntToDirection(selected[i]));
                    float temp = Vector2.SqrMagnitude(temppos - engine.player.prevpos);
                    if (temp < min)
                    {
                        sel = selected[i];
                        min = temp;
                    }
                }
            }
            NextPos = poses[sel];
        }
    }

    public override void Move()
    {
        if (Position != NextPos)
        {
            engine.RemovefromDatabase(this);
            Position = NextPos;
            transform.position = NextPos;
            engine.AddtoDatabase(this);
        }
        engine.EnemyMoveFinished();
    }
}
