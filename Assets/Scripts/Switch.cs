using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Unit{
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public End endtile { get; set; }
    public bool isOn = false;
    
    public void UnlockSound(){
        try
        {
            source.Play();
        }
        catch
        {
            source = GetComponent<AudioSource>();
        }
    }
}
