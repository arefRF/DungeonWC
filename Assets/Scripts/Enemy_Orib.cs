using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Orib : Enemy {
    private Animator animator;
    public float speed = 2;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public override void SetNextPos()
    {
        UpdatePlayerPos();
        if (PlayerPos == Position)
        {
            NextPos = Position;
            return;
        }
        List<int> selected = new List<int>();
        Vector2[] poses = new Vector2[4];
        for(int i=0; i<4; i++)
        {
            poses[i] = ToolKit.VectorSum(Position, ToolKit.IntToDirection(i));
        }
        poses[0] = ToolKit.VectorSum(poses[0], Direction.Right);
        poses[1] = ToolKit.VectorSum(poses[0], Direction.Down);
        poses[2] = ToolKit.VectorSum(poses[0], Direction.Left);
        poses[3] = ToolKit.VectorSum(poses[0], Direction.Up);
        float min = 10000;
        for (int i = 0; i < 4; i++)
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
        if (Position == NextPos)
        {
            engine.EnemyMoveFinished();
            return;
        }
        engine.AddToSnapshot(Clone());
        engine.RemovefromDatabase(this);
        Position = NextPos;
        animator.SetBool("Walk", true);
        StartCoroutine(MoveCo(NextPos)); 
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
        animator.SetBool("Walk", false);
        //animator.SetBool("KillWalk", false);
        engine.AddtoDatabase(this);
        engine.EnemyMoveFinished();

    }

    public override Clonable Clone()
    {
        return new ClonableEnemy_Orib(this);
    }
}

public class ClonableEnemy_Orib : Clonable
{
    public ClonableEnemy_Orib(Enemy_Orib enemy)
    {
        original = enemy;
        trasformposition = enemy.transform.position;
        position = enemy.Position;
    }

    public override void Undo()
    {
        Enemy_Orib enemy = original as Enemy_Orib;
        enemy.engine.RemovefromDatabase(original);
        enemy.Position = position;
        enemy.transform.position = trasformposition;
        enemy.engine.AddtoDatabase(original);
    }
}
