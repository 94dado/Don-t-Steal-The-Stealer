using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonActivator : MonoBehaviour {



	// Use this for initialization
	void Start () {
        if (PlayerPrefs.GetInt("GameStarted") == 1)
            this.GetComponent<Button>().interactable = true;
	}
	
	
}
