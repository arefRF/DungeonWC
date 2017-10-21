using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : Unit {

    public bool IsLocked = false;
    public bool hasSwitch = false;
	// Use this for initialization
	void Awake () {
        
        if (IsLocked)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            if(hasSwitch)
                transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            else
                transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
        }
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void Lock()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        IsLocked = true;
        if (hasSwitch)
        {
            Debug.Log("player switch lock animation");
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        }
        else
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Unlock()
    {
        IsLocked = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        if (hasSwitch)
        {
            Debug.Log("player switch unlock animation");
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        }
        else
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
    }
}
