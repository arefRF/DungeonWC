﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Simple : Enemy {
    public float speed = 2;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
        Load_Sounds();
        sound_detect = SearchSound("ex");
        sound_question = SearchSound("question");
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
            movedir = ToolKit.IntToDirection(selected[0]);
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
            movedir = ToolKit.IntToDirection(sel);
            NextPos = ToolKit.VectorSum(Position, ToolKit.IntToDirection(sel));
        }
    }

    public override void Move()
    {
        if (movedir == Direction.Right)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (movedir == Direction.Left)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        if (Position == NextPos)
        {
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            engine.EnemyMoveFinished();
            return;
        }
        if(PlayerPos == engine.player.Position)
        {
            if (!transform.GetChild(1).GetComponent<SpriteRenderer>().enabled)
            {
                source.PlayOneShot(sound_detect);
            }
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            if (!transform.GetChild(2).GetComponent<SpriteRenderer>().enabled)
                source.PlayOneShot(sound_question);
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
        }
        engine.AddToSnapshot(Clone());
        engine.RemovefromDatabase(this);
        Position = NextPos;
        npos = Position;
        if (key != null)
        {
            engine.AddToSnapshot(key.Clone());
            engine.RemovefromDatabase(key);
            key.Position = Position;
            engine.AddtoDatabase(key);
        }
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
            if(key != null)
            {
                key.transform.position = Vector3.MoveTowards(key.transform.position, nextPos, Time.deltaTime * speed);
            }
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

    public override void Die()
    {
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
        GetComponentInChildren<Animator>().SetBool("Death", true);
        engine.RemovefromDatabase(this);
        Clone();
        isdead = true;
    }
    protected AudioClip SearchSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
            if (sounds[i].name == name)
                return sounds[i];
        return null;
    }
    protected void Load_Sounds()
    {
        sounds = Resources.LoadAll<AudioClip>("Sounds\\Enemies");
    }
}

public class ClonableEnemy_Simple : Clonable
{
    public Vector2 playerpos;
    public bool isdead, b1, b2;
    public ClonableEnemy_Simple(Enemy_Simple enemy)
    {
        original = enemy;
        trasformposition = enemy.transform.position;
        position = enemy.Position;
        playerpos = enemy.PlayerPostemp;
        isdead = enemy.isdead;
        b1 = enemy.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled;
        b2 = enemy.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled;
    }

    public override void Undo()
    {
        Enemy_Simple enemy = original as Enemy_Simple;
        enemy.StopAllCoroutines();
        enemy.engine.RemovefromDatabase(original);
        enemy.Position = position;
        enemy.transform.position = trasformposition;
        enemy.engine.AddtoDatabase(original);
        enemy.PlayerPos = playerpos;
        enemy.GetComponentInChildren<Animator>().SetBool("Death", false);
        enemy.isdead = isdead;
        enemy.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = b1;
        enemy.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = b2;
    }
}
