using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : Unit {

    public bool IsLocked = false;
    Sprite lockicon;
	// Use this for initialization
	void Awake () {
        lockicon = Resources.Load<Sprite>("Branch Lock Icon");
        if (IsLocked)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void Lock()
    {
       
    }

    public void Unlock()
    {
        
    }
}
