using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        
        Unit unit = col.GetComponent<Unit>();
        if (unit is Player)
        {
            unit.GetComponentInChildren<Animator>().SetInteger("Death", 1);
        }
        else if (unit is Enemy)
        {
            if (unit == transform.parent.GetComponent<Enemy>())
                return;
                unit.GetComponentInChildren<Animator>().SetBool("Death",true);
        }
        else if(unit is Box)
        {
            Debug.Log("box destroy");
        }
        else if(unit is TNT)
        {
            Debug.Log("tnt boom");
        }
        transform.parent.gameObject.GetComponent<Enemy_Mage>().StopAllCoroutines();
        GetComponent<SpriteRenderer>().enabled = false;
        transform.localPosition = new Vector3(-2f, 2.2f, 10);
    }
}
