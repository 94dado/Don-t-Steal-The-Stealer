using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject interactionText;
    public GameObject missingGadgetText;
    public Text moneyText;
    public Text timeText;
    [HideInInspector]
    // check if we are in game over
    public bool gameOver;
    [HideInInspector]
    // check if we are win
    public bool win;

    [HideInInspector]
    public int oldObtainedMoney;
    [HideInInspector]
    public int newObtainedMoney;

    [HideInInspector]
    public int obtainedObjects;
    [HideInInspector]
    public int obtainableObjects;
    [HideInInspector]
    public float time;
    

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        oldObtainedMoney = 0;
        newObtainedMoney = 0;
        obtainedObjects = 0;
        obtainableObjects = getInteractableObjectsNumber(LayerMask.NameToLayer("Interactable"));
    }

    private void Update()
    {
        //keep increasing timer untill the game has finished
        if (!gameOver && !win)
            showTimer();
        if(oldObtainedMoney < newObtainedMoney)
            updateMoney();
    }

    //activate the "press E to interact" text
    public void ActivateInteractionText(bool active)
    {
        interactionText.SetActive(active);
    }
    //activate the "missing gadget" text
    public void ActivateNoGadgetText(bool active)
    {
        missingGadgetText.SetActive(active);
    }

    //add money to the total balance
    public void AddMoney(int money,string tag)
    {
        newObtainedMoney = newObtainedMoney +money;
        

        if (tag != "Door")
            obtainedObjects = obtainedObjects + 1;
    }

    //used when creating the gameOver menu, coounts all interactable objects
    private int getInteractableObjectsNumber(int layer)
    {
        int interactableObjectNumber = 0;
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer && goArray[i].tag != "Door")
            {
                interactableObjectNumber = interactableObjectNumber + 1;
            }
        }
        return interactableObjectNumber;
    }

    //shows the timer in a hh:mm:ss 
    private void showTimer()
    {
        int showedSeconds;
        int showedMinutes;
        int showedHours;

        //calculate time in hours:minutes:seconds
        time = time + Time.deltaTime;
        int remainingTime = (int)Mathf.Floor(time);
        showedHours = remainingTime / 3600;
        remainingTime = remainingTime - showedHours * 3600;
        showedMinutes = remainingTime / 60;
        remainingTime = remainingTime - showedMinutes * 60;
        showedSeconds = remainingTime;


        string showedSecondText;
        string showedMinutesText;
        string showedHoursText;
        // adding a 0 in front of time if is less than 9
        if (showedSeconds <= 9)
            showedSecondText = "0" + showedSeconds.ToString();
        else
            showedSecondText = showedSeconds.ToString();

        if (showedMinutes <= 9)
            showedMinutesText = "0" + showedMinutes.ToString();
        else
            showedMinutesText = showedMinutes.ToString();

        if (showedHours <= 9)
            showedHoursText = "0" + showedHours.ToString();
        else
            showedHoursText = showedHours.ToString();
        //display time
        if (showedHours == 0)
                timeText.text =  showedMinutesText + ":" + showedSecondText;
            else
                timeText.text = showedHoursText+":"+showedMinutesText + ":" + showedSecondText;

    }

    //update money in order to display it incrementally
    private void updateMoney()
    {
        oldObtainedMoney = oldObtainedMoney + 3;
        if (oldObtainedMoney > newObtainedMoney)
            oldObtainedMoney = newObtainedMoney;
        moneyText.text = "Money: " + oldObtainedMoney + "$";
    }
}
