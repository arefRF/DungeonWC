using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseHome : MonoBehaviour {
    public GameObject home;
    void OnTriggerEnter2D(Collider2D col){
        if (col.name == "Player")
        {
            home.GetComponent<Animator>().SetTrigger("Collapse");
        }
    }
}
