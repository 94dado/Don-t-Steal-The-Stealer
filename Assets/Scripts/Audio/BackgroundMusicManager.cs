using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour {

    //audio component
    AudioSource audioManager;
    //index of current playing song
    int current;

    //list of all audio of the level
    public AudioClip[] audioList;
    //the index of the first song to play
    public int startOffset = 0;

    // Use this for initialization
    void Start () {
        audioManager = GetComponent<AudioSource>();
        current = startOffset;
        audioManager.clip = audioList[current];
        StartCoroutine("BackgroundMusic");
	}

    IEnumerator BackgroundMusic() {
        while (true) {
            audioManager.Play();
            yield return new WaitForSeconds(audioManager.clip.length);
            current = (current + 1) % (audioList.Length - 1);
            AudioClip nextClip = audioList[current];
            audioManager.clip = nextClip;
        }
    }

    public void StopMusic(bool win) {
        StopCoroutine("BackgroundMusic");
        //TODO: change audio in base of win value and play that
    }
}
