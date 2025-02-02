﻿using System.Collections;
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
            if (SceneManager.GetActiveScene().name == "Video")
                Music.instance.source.Stop();
            else if (SceneManager.GetActiveScene().name == "Intro")
            {
                AudioClip music = SearchMusic("Home");
                Music.instance.source.PlayOneShot(music);
            }
            else
            {
                AudioClip music = SearchMusic("Menu");
                Music.instance.source.Stop();
                Music.instance.StopAllCoroutines();
                Music.instance.source.PlayOneShot(music);
                StartCoroutine(MusicShuffle(music.length));
            }
            Destroy(this.gameObject);
            return;
        }
        else
        {

            
            instance = this;
            source = GetComponent<AudioSource>();
            Load_Musics();
            int i = Random.Range(1, 3);
            if (SceneManager.GetActiveScene().name == "Video")
                Music.instance.source.Stop();
            else if (SceneManager.GetActiveScene().name == "Intro")
            {
                AudioClip music = SearchMusic("Home");
                Music.instance.source.PlayOneShot(music);
            }
            else
            {
                AudioClip music = SearchMusic("Menu");
                Music.instance.source.Stop();
                Music.instance.StopAllCoroutines();
                Music.instance.source.PlayOneShot(music);
                StartCoroutine(MusicShuffle(music.length));
            }
        }
       

        if (SceneManager.GetActiveScene().name == "Start")
        {
            AudioClip music = SearchMusic("Menu");
            Music.instance.source.PlayOneShot(music);
            StartCoroutine(MusicShuffle(music.length));
        }
        

        DontDestroyOnLoad(this.gameObject);
	}

    private AudioClip SearchMusic(string name){
        for(int i=0;i<Music.instance.sounds.Length;i++)
            if(Music.instance.sounds[i].name == name)
                return Music.instance.sounds[i];
        return null;
    }
    IEnumerator MusicShuffle(float time)
    {
        yield return new WaitForSeconds(time);
        int i = Random.Range(1, 3);
        AudioClip music = SearchMusic("Menu");
        Music.instance.source.Stop();
        Music.instance.source.PlayOneShot(music);
        StartCoroutine(MusicShuffle(music.length));
    }
    void Load_Musics()
    {
        sounds = Resources.LoadAll<AudioClip>("Musics");
    }
	
}
