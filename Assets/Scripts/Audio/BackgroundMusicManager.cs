using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour {
    
    // audio of the background
    public AudioClip menuAudio;
    public AudioClip winAudio;
    public AudioClip gameoverAudio;
    //list of all audio of the level
    public AudioClip[] levelAudios;
    //the index of the first song to play
    public int startOffset;

    //audio component
    AudioSource audioManager;
    //index of current playing song
    int current;
    bool coroutineIsStarted;
    bool wasLevel;

    void Awake() {
        MakePersistent();
    }

    // Use this for initialization
    void Start () {
        audioManager = GetComponent<AudioSource>();
        current = startOffset;
        wasLevel = GameObject.FindWithTag("Crosshair") != null;
	}

    void Update() {
        // check if the scene is not changed
        bool isLevel = GameObject.FindWithTag("Crosshair") != null;
        bool isSameLevel = isLevel == wasLevel;
        // if we are in a level
        if ((isSameLevel && isLevel && !coroutineIsStarted) || (!isSameLevel && isLevel)) {
            StartNewMusic(levelAudios[current]);
        }
        // if we are in a menu
        if ((isSameLevel && !isLevel && !coroutineIsStarted) || (!isSameLevel && !isLevel)) {
            StartNewMusic(menuAudio);
        }
        // if gameover
        if (GameManager.instance.gameOver) {
            StartNewMusic(gameoverAudio);
        }
        // if win
        if (GameManager.instance.win) {
            StartNewMusic(winAudio);
        }
        wasLevel = isLevel;
    }

    // make the gameobject persistent between scenes
    void MakePersistent() {
        // remove the copy of the scene controller if exist
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioManager");
        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }
        // save the scene after the reload
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator BackgroundMusic() {
        coroutineIsStarted = true;
        audioManager.Play();
        yield return new WaitForSeconds(audioManager.clip.length);
        current = (current + 1) % (levelAudios.Length - 1);
        coroutineIsStarted = false;
    }

    // start a new song
    void StartNewMusic(AudioClip audioClip) {
        StopCoroutine("BackgroundMusic");
        audioManager.clip = audioClip;
        StartCoroutine("BackgroundMusic");
    }
}
