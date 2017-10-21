using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {

        Unit unit = col.GetComponent<Unit>();
        if (col.name == "FireBall")
            return;
        if (unit is Player)
        {
            (unit as Player).Die();
        }
        else if (unit is Enemy)
        {
            if (unit == transform.parent.GetComponent<Enemy>())
                return;
            (unit as Enemy).Die();
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
        transform.parent.gameObject.GetComponent<Enemy_Mage>().shootingfireball = false;
        transform.parent.gameObject.GetComponent<Enemy_Mage>().engine.EnemyMoveFinished();
        GetComponent<SpriteRenderer>().enabled = false;
        transform.localPosition = new Vector3(-1.15f, 0.69f, 10);
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
