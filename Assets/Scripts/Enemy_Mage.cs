using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mage : Enemy {

    public bool FireballCharged { get; set; }
    public Direction chargedirection { get; set; }
    private Animator animator;
    public float speed = 3;
    public bool shootingfireball { get; set; }

    void Start()
    {
        FireballCharged = false;
        shootingfireball = false;
        animator = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
        Load_Sounds();
        sound_detetct = SearchSound("Monster 1");
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
            {
                Debug.Log("asf32323232323232323");
                engine.EnemyMoveFinished();
            }
            else if (selected.Count == 1)
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
            if (PlayerPos == engine.player.Position)
            {
                if(!transform.GetChild(1).GetComponent<SpriteRenderer>().enabled)
                    source.PlayOneShot(sound_detetct);
                transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                //if(!transform.GetChild(2).GetComponent<SpriteRenderer>().enabled)
                transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            }
            engine.AddToSnapshot(Clone());
            engine.RemovefromDatabase(this);
            Position = NextPos;
            if (key != null)
            {
                engine.AddToSnapshot(key.Clone());
                engine.RemovefromDatabase(key);
                key.Position = Position;
                key.transform.position = Position;
                engine.AddtoDatabase(key);
            }
            animator.SetBool("Walk", true);
            StartCoroutine(MoveCo(NextPos));
            // transform.position = NextPos;

        }
        else if(!shootingfireball)
        {
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            engine.EnemyMoveFinished();
        }
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
        shootingfireball = true;
        transform.GetChild(3).GetComponent<CircleCollider2D>().enabled = true;
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
        GameObject g = transform.GetChild(3).gameObject;
        Vector3 nextpos = new Vector3(0,0, 10);
        switch (direction)
        {
            case Direction.Down: nextpos = new Vector3(g.transform.position.x, 0, 10); break;
            case Direction.Left: nextpos = new Vector3(0, g.transform.position.y, 10); break;
            case Direction.Right: nextpos = new Vector3(engine.sizeX, g.transform.position.y, 10); break;
            case Direction.Up: nextpos = new Vector3(g.transform.position.x, engine.sizeY, 10); break;
        }
        g.GetComponent<SpriteRenderer>().enabled = true;
        while (g.transform.position.x > 0 && g.transform.position.y > 0 && g.transform.position.x < engine.sizeX && g.transform.position.y < engine.sizeY)
        {
            g.GetComponent<SpriteRenderer>().enabled = true;
            g.transform.position = Vector3.MoveTowards(g.transform.position, nextpos, Time.deltaTime * 15);
            yield return null;
        }
        shootingfireball = false;
        g.GetComponent<SpriteRenderer>().enabled = false;
        g.transform.localPosition = new Vector3(-2f, 2.2f, 10);
        transform.GetChild(3).GetComponent<CircleCollider2D>().enabled = false;
        engine.EnemyMoveFinished();
    }

    public override Clonable Clone()
    {
        return new ClonableEnemy_Mage(this);
    }

    public override void Die()
    {
        Clone();
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
        GetComponentInChildren<Animator>().SetBool("Death", true);
        engine.RemovefromDatabase(this);
        isdead = true;
    }
}

public class ClonableEnemy_Mage : Clonable
{
    public Direction fireballdirection;
    public bool charged;
    public Vector2 playerpos;
    public bool isdead, b1, b2;
    public ClonableEnemy_Mage(Enemy_Mage enemy)
    {
        original = enemy;
        trasformposition = enemy.transform.position;
        position = enemy.Position;
        charged = enemy.FireballCharged;
        fireballdirection = enemy.chargedirection;
        playerpos = enemy.PlayerPostemp;
        isdead = enemy.isdead;
        b1 = enemy.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled;
        b2 = enemy.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled;
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
        if(!charged)
            enemy.GetComponentInChildren<Animator>().SetBool("Charge", false);
        else
            enemy.GetComponentInChildren<Animator>().SetBool("Charge", true);
        enemy.chargedirection = fireballdirection;
        enemy.PlayerPos = playerpos;
        enemy.GetComponentInChildren<Animator>().SetBool("Death", false);
        enemy.isdead = isdead;
        enemy.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = b1;
        enemy.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = b2;
    }
}
