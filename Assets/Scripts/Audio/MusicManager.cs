using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound {

    public string name;
    public AudioClip clip;
    // the first song to start at the start of the game
    public bool firstSong;

    [HideInInspector]
    public AudioSource source;

    public void SetSource(AudioSource source) {
        this.source = source;
        source.clip = clip;
    }

    public void Play() {
        source.Play();
    }

    public void Stop() {
        source.Stop();
    }
}

public class MusicManager : MonoBehaviour {

    public string[] levelSongsName;
    public string winningSongName;
    public string gameoverSongName;
    public string menuSongName;

    [SerializeField]
    Sound[] sounds;

    // current song
    List<Sound> backgroundSounds = new List<Sound>();
    int currentLevelSong;

    void Awake() {
        MakePersistent();
    }

	// Use this for initialization
	void Start () {
        PlayFirstSound();
	}

    // play the first song
    void PlayFirstSound() {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].firstSong) {
                sounds[i].Play();
                return;
            }
        }
    }

    // play an audio
    public void PlaySound(string songName) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == songName) {
                sounds[i].Play();
                return;
            }
        }
    }

    // stop all audio
    void StopSounds() {
        for (int i = 0; i < sounds.Length; i++) {
            sounds[i].Stop();
        }
        backgroundSounds.Clear();
    }

    // get one of the sounds by name
    Sound GetSound(string songName) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == songName) {
                return sounds[i];
            }
        }
        return null;
    }

    // make the gameobject persistent between scenes
    void MakePersistent() {
        // remove the copy of the scene controller if exist
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioManager");
        if (objs.Length > 1) {
            Destroy(gameObject);
        }
        // save the scene after the reload
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < sounds.Length; i++) {
            GameObject go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            sounds[i].SetSource(go.AddComponent<AudioSource>());
            DontDestroyOnLoad(go);
        }
    }

    // Update is called once per frame
    void Update () {
        CheckSongs();
        // check if we are in level scene
        bool isLevel = GameObject.FindWithTag("Crosshair") != null;
        // if level
        if (isLevel && !IsLevelSongPlaying()) {
            StopSounds();
            StartCoroutine("LevelMusic");
        }
        // if menu
        if (!isLevel && !backgroundSounds.Contains(GetSound(menuSongName))) {
            StopCoroutine("LevelMusic");
            StopSounds();
            PlaySound(menuSongName);
        }
	}

    // play a song until the end and after pass to next
    IEnumerator LevelMusic() {
        PlaySound(levelSongsName[currentLevelSong]);
        yield return new WaitForSeconds(GetSound(levelSongsName[currentLevelSong]).clip.length);
        currentLevelSong = (currentLevelSong + 1) % (levelSongsName.Length - 1);
    }

    // check playing audio
    void CheckSongs() {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].source.isPlaying) {
                backgroundSounds.Add(sounds[i]);
            }
        }
    }

    // check if a level song is playing
    bool IsLevelSongPlaying() {
        for (int i = 0; i < levelSongsName.Length; i++) {
            if (backgroundSounds.Contains(GetSound(levelSongsName[i]))) {
                return true;
            }
        }
        return false;
    }
}
