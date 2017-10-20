using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseHome : MonoBehaviour {
    public GameObject home;
    void OnTriggerEnter2D(Collider2D col){
        if (col.name == "Player")
        {
            col.GetComponentInChildren<Animator>().SetTrigger("Fall");
            home.GetComponent<Animator>().SetTrigger("Collapse");
            
        }
    }
}
