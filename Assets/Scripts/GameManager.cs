using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject interactionText;
    public GameObject missingGadgetText;
    public Text moneyText;
    private int obtainedMoney;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //activate the "press E to interact" text
    public void ActivateInteractionText(bool active)
    {
        interactionText.SetActive(active);
    }
    public void ActivateNoGadgetText(bool active)
    {
        missingGadgetText.SetActive(active);
    }

    //add money to the total balance
    public void AddMoney(int money)
    {
        obtainedMoney = obtainedMoney +money;
        moneyText.text = "Money : " + obtainedMoney;
    }
}
