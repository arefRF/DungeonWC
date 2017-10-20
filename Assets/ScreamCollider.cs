using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamCollider : MonoBehaviour {
    private AudioSource source;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
    void OnTriggerEnter2D(Collider2D col){
        if(col.name == "Player")
            source.Play();
    }
}
