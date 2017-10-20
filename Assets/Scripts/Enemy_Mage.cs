using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mage : Enemy {

    public bool FireballCharged { get; set; }
    public Direction chargedirection { get; set; }
    private Animator animator;
    public float speed = 3;

    void Start()
    {
        FireballCharged = false;
        animator = GetComponentInChildren<Animator>();
    }
    public override void SetNextPos()
    {
        if (FireballCharged)
        {
            ShootFireBall(chargedirection);
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
                if(temppos.x == 0)
                {
                    if(temppos.y > 0)
                        ChargeFireBall(Direction.Up);
                    else
                        ChargeFireBall(Direction.Down);
                }
                else if (temppos.y == 0)
                {
                    if (temppos.x > 0)
                        ChargeFireBall(Direction.Right);
                    else
                        ChargeFireBall(Direction.Left);
                }
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
            engine.AddToSnapshot(Clone());
            engine.RemovefromDatabase(this);
            Position = NextPos;
            animator.SetBool("Walk", true);
            StartCoroutine(MoveCo(NextPos));
           // transform.position = NextPos;
           
        }
        else
            engine.EnemyMoveFinished();
    }

    public void ChargeFireBall(Direction direction)
    {
        engine.AddToSnapshot(Clone());
        chargedirection = direction;
        FireballCharged = true;
        animator.SetBool("Charge",true);
    }

    public void ShootFireBall(Direction direction)
    {
        animator.SetBool("Charge", false);
        GameObject g = transform.GetChild(1).gameObject;
        g.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(MoveCoFireBall(direction));
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
    private IEnumerator MoveCoFireBall(Direction direction)
    {
        GameObject g = transform.GetChild(1).gameObject;
        Vector2 nextpos = new Vector2(0,0);
        switch (direction)
        {
            case Direction.Down: nextpos = new Vector2(g.transform.position.x, 0); break;
            case Direction.Left: nextpos = new Vector2(0, g.transform.position.y); break;
            case Direction.Right: nextpos = new Vector2(engine.sizeX, g.transform.position.y); break;
            case Direction.Up: nextpos = new Vector2(g.transform.position.x, engine.sizeY); break;
        }
        while (g.transform.position.x > 0 && g.transform.position.y > 0 && g.transform.position.x < engine.sizeX && g.transform.position.y < engine.sizeY)
        {
            g.transform.position = Vector3.MoveTowards(g.transform.position, nextpos, Time.deltaTime * 15);
            //Debug.Log("aslb");
            yield return null;
        }
        g.GetComponent<SpriteRenderer>().enabled = false;
        g.transform.localPosition = new Vector3(-2f, 2.2f, 10);

    }

    public override Clonable Clone()
    {
        return new ClonableEnemy_Mage(this);
    }
}

public class ClonableEnemy_Mage : Clonable
{
    public Direction fireballdirection;
    public bool charged;
    public Vector2 playerpos;
    public ClonableEnemy_Mage(Enemy_Mage enemy)
    {
        original = enemy;
        trasformposition = enemy.transform.position;
        position = enemy.Position;
        charged = enemy.FireballCharged;
        fireballdirection = enemy.chargedirection;
        playerpos = enemy.PlayerPos;
    }

    public override void Undo()
    {
        Enemy_Mage enemy = original as Enemy_Mage;
        enemy.StopAllCoroutines();
        GameObject g = enemy.transform.GetChild(1).gameObject;
        g.GetComponent<SpriteRenderer>().enabled = false;
        g.transform.localPosition = new Vector3(-2f, 2.2f, 10);
        enemy.engine.RemovefromDatabase(original);
        enemy.Position = position;
        enemy.transform.position = trasformposition;
        enemy.engine.AddtoDatabase(original);
        enemy.FireballCharged = charged;
        enemy.chargedirection = fireballdirection;
        enemy.PlayerPos = playerpos;
    }
}
