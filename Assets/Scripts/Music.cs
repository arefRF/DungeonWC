using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Music : MonoBehaviour {
    private static Music instance = null;
    private AudioSource source;
    private AudioClip[] sounds;
	// Use this for initialization
	void Start () {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            source = GetComponent<AudioSource>();
            Load_Musics();
            int i = Random.Range(1, 3);
            if (SceneManager.GetActiveScene().name == "Start")
            {
                Music.instance.source.PlayOneShot(Music.instance.sounds[0]);
                StartCoroutine(MusicShuffle(Music.instance.sounds[0].length));
            }
            else
            {
                Music.instance.source.PlayOneShot(Music.instance.sounds[i]);
                StartCoroutine(MusicShuffle(Music.instance.sounds[i].length));
            }
          
        }

        DontDestroyOnLoad(this.gameObject);
	}
    IEnumerator MusicShuffle(float time)
    {
        yield return new WaitForSeconds(time);
        int i = Random.Range(1, 3);
        Music.instance.source.Stop();
        Music.instance.source.PlayOneShot(Music.instance.sounds[i]);
        StartCoroutine(MusicShuffle(Music.instance.sounds[i].length));
    }
    void Load_Musics()
    {
        sounds = Resources.LoadAll<AudioClip>("Musics");
        Music.instance.source.volume = 0.4f;
    }
	
}
