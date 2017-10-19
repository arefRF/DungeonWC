using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mage : Enemy {

    private bool FireballCharged = false;

    public override void SetNextPos()
    {
        if (FireballCharged)
        {
            ShootFireBall();
            FireballCharged = false;
            return;
        }
        UpdatePlayerPos();
        if (PlayerPos == Position)
        {
            NextPos = Position;
            return;
        }
        if (engine.player.Position != PlayerPos)
        {
            List<int> selected = new List<int>();
            float min = 10000;
            for (int i = 0; i < 4; i++)
            {
                Vector2 temppos = ToolKit.VectorSum(Position, ToolKit.IntToDirection(i));
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
            if (selected.Count == 0)
                engine.EnemyMoveFinished();
            if (selected.Count == 1)
            {
                NextPos = ToolKit.VectorSum(Position, ToolKit.IntToDirection(selected[0]));
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
                NextPos = ToolKit.VectorSum(Position, ToolKit.IntToDirection(sel));
            }
        }
        else
        {
            Vector2 temppos = PlayerPos - Position;
            if(temppos.x == 0 || temppos.y == 0)
            {
                ChargeFireBall();
                NextPos = Position;
                return;
            }
            if(Mathf.Abs(temppos.x) < Mathf.Abs(temppos.y))
            {
                if (PlayerPos.x > Position.x)
                    NextPos = ToolKit.VectorSum(Position, Direction.Right);
                else
                    NextPos = ToolKit.VectorSum(Position, Direction.Left);

            }
            else
            {
                if (PlayerPos.y > Position.y)
                    NextPos = ToolKit.VectorSum(Position, Direction.Up);
                else
                    NextPos = ToolKit.VectorSum(Position, Direction.Down);
            }
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

    public void ChargeFireBall()
    {

    }

    public void ShootFireBall()
    {

    }
}
