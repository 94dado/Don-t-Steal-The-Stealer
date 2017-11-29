using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject interactionText;
    public GameObject missingGadgetText;
    public Text moneyText;
    [HideInInspector]
    // check if we are in game over
    public bool gameOver;
    [HideInInspector]
    // check if we are win
    public bool win;

    int obtainedMoney;
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
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
