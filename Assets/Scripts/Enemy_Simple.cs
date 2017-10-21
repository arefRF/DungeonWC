using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Simple : Enemy {
    public float speed = 2;
    private Animator animator;

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
        float min = 10000;
        for(int i=0; i<4; i++)
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
        if(selected.Count == 1)
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
        if (engine.player.Position == NextPos)
            animator.SetBool("KillWalk", true);
        else
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
        animator.SetBool("KillWalk", false);
        engine.AddtoDatabase(this);
        engine.EnemyMoveFinished();
        
    }

    public override Clonable Clone()
    {
        return new ClonableEnemy_Simple(this);
    }
}

public class ClonableEnemy_Simple : Clonable
{
    public ClonableEnemy_Simple(Enemy_Simple enemy)
    {
        original = enemy;
        trasformposition = enemy.transform.position;
        position = enemy.Position;
    }

    public override void Undo()
    {
        Enemy_Simple enemy = original as Enemy_Simple;
        enemy.StopAllCoroutines();
        enemy.engine.RemovefromDatabase(original);
        enemy.Position = position;
        enemy.transform.position = trasformposition;
        enemy.engine.AddtoDatabase(original);
    }
}
