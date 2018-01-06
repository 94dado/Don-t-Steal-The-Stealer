using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonActivator : MonoBehaviour {



	// Use this for initialization
	void Start () {
        DataManager dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        if (dataManager.AlreadyStarted == true)
            this.GetComponent<Button>().interactable = true;
	}
	
	
}
