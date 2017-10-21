using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

    public Vector2 NextPos { get; set; }
    public Key key { get; set; }
    public Vector2 PlayerPos { get; set; }
    public Vector2 PlayerPostemp { get; set; }
    protected AudioClip[] sounds;
    protected AudioClip sound_detetct;
    protected AudioSource source;
    public bool isdead { get; set; }

    void Start()
    {
        source = GetComponent<AudioSource>();
        Load_Sounds();
        sound_detetct = SearchSound("Monster 1");
    }
    public virtual void SetNextPos()
    {
       
    }

	public virtual void Move()
    {
         
    }

    public virtual void ForceMove()
    {
        StopAllCoroutines();
        transform.position = npos;
        engine.EnemyMoveFinished();
    }

    public virtual bool CanMoveToPosition(Vector2 position)
    {
        if(!(position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY))
            return false;
        List<Unit> units = engine.units[(int)position.x, (int)position.y];
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] is Box || units[i] is Block ||  units[i] is TNT)
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
                source.PlayOneShot(sound_detetct);
                PlayerPos = player.Position;
                transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    public virtual void Die()
    {
        
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
