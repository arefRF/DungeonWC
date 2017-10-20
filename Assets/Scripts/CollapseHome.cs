using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseHome : MonoBehaviour {
    public GameObject home;
    public GameObject fallSound;
    void OnTriggerEnter2D(Collider2D col){
        if (col.name == "Player")
        {
            home.GetComponent<AudioSource>().loop = false;
            home.GetComponent<AudioSource>().Play();
            StartCoroutine(Wait(col.gameObject));

        }
    }

    private IEnumerator Wait(GameObject col)
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(wait());
        col.GetComponentInChildren<Animator>().SetTrigger("Fall");
        fallSound.GetComponent<AudioSource>().Play();
        home.GetComponent<Animator>().SetTrigger("Collapse");
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("000");
    }
}
