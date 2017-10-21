using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSence : MonoBehaviour {

    public string SceneName;

    private bool b = false;

    void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

	public void ChangeScene()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("here");
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }
}
