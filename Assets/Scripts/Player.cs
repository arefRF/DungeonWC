﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {

    public Key key { get; set; }
    public Box box { get; set; }
    public TNT tnt { get; set; }
    private Animator animator;
    private AnimationEventPlayer a_event;
    public Vector2 prevpos { get; set; }
    public float speed = 3;
    private SpriteRenderer sprite;
    private AudioClip[] sounds;
    private AudioClip death_sound;
    public bool isdead = false;
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        a_event = GetComponentInChildren<AnimationEventPlayer>();
        animator = GetComponentInChildren<Animator>();
        Load_Sounds();
        death_sound = SearchSound("Player Death");
    }

    /*public void Move(Direction direction)
    {
        engine.RemovefromDatabase(this);
        Vector2 temppos = ToolKit.VectorSum(Position, direction);
        bool playermoved = false;
        if (CanMoveToPosition(temppos, direction))
        {
            engine.AddToSnapshot(Clone());
            prevpos = Position;
            Position = temppos;
            animator.SetInteger("Walk", 0);
            if (key != null)
            {
                engine.AddToSnapshot(key.Clone());
                engine.RemovefromDatabase(key);
                key.Position = ToolKit.VectorSum(key.Position, direction);
                key.transform.position = ToolKit.VectorSum(key.transform.position, direction);
                engine.AddtoDatabase(key);
            }
            if (box != null)
            {
                box.Move(direction);
                box = null;
            }
            if(tnt != null)
            {
                tnt.Move(direction);
                tnt = null;
            }
            playermoved = true;
            engine.AddtoDatabase(this);
        }
        else
        {
            
            engine.AddtoDatabase(this);
            MoveFinished(playermoved);
        }
    }*/

    private void GraphicMove(Direction dir)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        if(dir == Direction.Left)
            transform.rotation = Quaternion.Euler(0,180,0);
        else
            transform.rotation = Quaternion.Euler(0,0,0);
        if (dir == Direction.Left || dir == Direction.Right)
        {
            sprite.sprite = (Sprite)Resources.Load("Player\\Walk 1", typeof(Sprite));
            //SetInteger("Walk", 1);
        }
        else if (dir == Direction.Up)
            sprite.sprite = (Sprite)Resources.Load("Player\\Walk 3", typeof(Sprite));
            //animator.SetInteger("Walk", 2);
        else if (dir == Direction.Down)
            sprite.sprite = (Sprite)Resources.Load("Player\\Walk 2",typeof(Sprite));
            //animator.SetInteger("Walk", 3);
        StartCoroutine(MovePlayerCo(dir));
    }

    private IEnumerator MovePlayerCo(Direction dir)
    {
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(true);
        sprite.sprite = null;
        transform.position = ToolKit.VectorSum(transform.position, dir);
        MoveFinished(true);
    }

    private void BoxMoveAnimation(Direction dir)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        if (dir == Direction.Left)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
        if (dir == Direction.Right || dir == Direction.Left)
        {
            sprite.sprite = (Sprite)Resources.Load("Player\\Box 1", typeof(Sprite));
        }
        else if (dir == Direction.Up)
            sprite.sprite = (Sprite)Resources.Load("Player\\Box 3", typeof(Sprite));
        else if (dir == Direction.Down)
            sprite.sprite = (Sprite)Resources.Load("Player\\Box 2", typeof(Sprite));
    }

    private IEnumerator BoxMoveCo(Vector3 nextPos)
    {
        float remain = (transform.position - nextPos).sqrMagnitude;
        while (remain > float.Epsilon)
        {
            remain = (transform.position - nextPos).sqrMagnitude;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * speed);
            yield return null;
        }

        /// Move Finished
        transform.GetChild(0).gameObject.SetActive(true);
        sprite.sprite = null;
        engine.PlayerMoveFinished(true);
        engine.AddtoDatabase(this);
    }
    public void Move(Direction direction)
    {
        engine.RemovefromDatabase(this);
        Vector2 temppos = ToolKit.VectorSum(Position, direction);
        bool playermoved = false;
        if (CanMoveToPosition(temppos, direction))
        {
            npos = ToolKit.VectorSum(transform.position, direction);
            engine.AddToSnapshot(Clone());
            for (int i = 0; i < engine.enemies.Count; i++)
            {
                if(engine.enemies[i] is Enemy_Mage && (engine.enemies[i] as Enemy_Mage).FireballCharged)
                    engine.AddToSnapshot(engine.enemies[i].Clone());
            }
            prevpos = Position;
            Position = temppos;
            a_event.dir = direction;
            Vector2 nextPos = ToolKit.VectorSum(transform.position, direction);
            //transform.position = ToolKit.VectorSum(transform.position, direction);
            if (key != null)
            {
                engine.AddToSnapshot(key.Clone());
                engine.RemovefromDatabase(key);
                key.Position = ToolKit.VectorSum(key.Position, direction);
                key.transform.position = ToolKit.VectorSum(key.transform.position, direction);
                engine.AddtoDatabase(key);
            }

            if (box != null)
            {
                StartCoroutine(BoxMoveCo(nextPos));
                BoxMoveAnimation(direction);
                box.Move(direction);
                box = null;
            }
            else if (tnt != null)
            {
                tnt.Move(direction);
                tnt = null;
            }
            else
                GraphicMove(direction);

            playermoved = true;
        }
        else
        {
            MoveFinished(playermoved);
        }
    }

    public void ForceMove()
    {
        StopAllCoroutines();
        transform.position = npos;
        MoveFinished(true);
    }

    public void MoveFinished(bool playermoved)
    {
        engine.AddtoDatabase(this);
        animator.SetInteger("Walk", 0);
        engine.PlayerMoveFinished(playermoved);
    }


    /*public void FakeMove(Direction dir)
    {
        return;
        if (engine.turn == Turn.EnemyTurn)
            return;
        Vector2 temp = ToolKit.VectorSum(Position, dir);
        if (!CanMoveToPosition(temp, dir))
            return;
        List<Unit> units = engine.units[(int)temp.x, (int)temp.y];
        for (int i = 0; i < units.Count; i++)
        {
            if(units[i] is Box)
            {
                //player move box animation
                return;
            }
        }
        a_event.dir = dir;
        if (dir == Direction.Right)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("Walk", 1);
        }
        else if (dir == Direction.Left)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
            animator.SetInteger("Walk", 1);
        }

        else if (dir == Direction.Up)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("Walk", 2);
        }
        else if(dir == Direction.Down)
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("Walk", 3);
        }
    }*/
    private bool CanMoveToPosition(Vector2 position, Direction direction)
    {
        if (!(position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY))
            return false;
        List<Unit> units = engine.units[(int)position.x, (int)position.y];
        for(int i=0; i<units.Count; i++)
        {
            if (units[i] is Block)
                return false;
            if(units[i] is Box)
            {
                if ((units[i] as Box).CanMoveToPosition(ToolKit.VectorSum(position, direction), direction))
                {
                    box = units[i] as Box;
                    return true;
                }
                else
                    return false;
            }
            else if (units[i] is TNT)
            {
                if ((units[i] as TNT).CanMoveToPosition(ToolKit.VectorSum(position, direction), direction))
                {
                    tnt = units[i] as TNT;
                    return true;
                }
                else
                    return false;
            }
            /*else if (units[i] is Enemy)
            {
                return false;
            }*/
        }
        return true;
    }

    public override Clonable Clone()
    {
        return new ClonablePlayer(this);
    }

    public void Die()
    {
        source.PlayOneShot(death_sound);
        GetComponentInChildren<Animator>().SetBool("Death", true);
        engine.RemovefromDatabase(this);
        Clone();
        isdead = true;
        StartCoroutine(ShowAfterWait());
    }
    private IEnumerator ShowAfterWait()
    {
        yield return new WaitForSeconds(2f);
        GameObject.Find("Input").GetComponent<GetInput>().inMenuShow();
    }

    private AudioClip SearchSound(string name)
    {
        for (int i = 0; i <sounds.Length; i++)
            if (sounds[i].name == name)
                return sounds[i];
        return null;
    }
    void Load_Sounds()
    {
        sounds = Resources.LoadAll<AudioClip>("Sounds\\Player"); 
    }
}

public class ClonablePlayer: Clonable
{
    public Key key;
    public Vector2 prevpos;

    public ClonablePlayer(Player player)
    {
        original = player;
        position = player.Position;
        key = player.key;
        prevpos = player.prevpos;
        trasformposition = player.transform.position;
    }

    public override void Undo()
    {
        Player player = original as Player;
        player.engine.RemovefromDatabase(original);
        player.Position = position;
        player.key = key;
        player.prevpos = prevpos;
        player.transform.position = trasformposition;
        player.engine.AddtoDatabase(original);
        player.isdead = false;
        player.GetComponentInChildren<Animator>().SetBool("Death", false);
    }
}
