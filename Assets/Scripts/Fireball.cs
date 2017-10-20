using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        transform.parent.gameObject.GetComponent<Enemy_Mage>().StopAllCoroutines();
        GetComponent<SpriteRenderer>().enabled = false;
        transform.localPosition = new Vector3(-2f, 2.2f, 10);
        Unit unit = col.GetComponent<Unit>();
        if (unit is Player)
        {
            Debug.Log("player die");
        }
        else if (unit is Enemy)
        {
            Debug.Log("enemy diw");
        }
        else if(unit is Box)
        {
            Debug.Log("box destroy");
        }
        else if(unit is TNT)
        {
            Debug.Log("tnt boom");
        }
    }
}
