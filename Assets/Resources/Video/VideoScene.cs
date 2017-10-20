using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VideoScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Wait());
	}
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            SceneManager.LoadScene("Intro");
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(25);
        SceneManager.LoadScene("Intro");
    }
}
