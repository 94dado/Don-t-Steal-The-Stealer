using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    
    public Text screenText;
    
    public GameOverManager gameOverManager;
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

    //list of all interactable that aren't stealable
    public string[] notStealable;
    

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
        else if (gameOver)
            gameOverManager.activateGameOverMenu();
        else
            gameOverManager.activateVictoryMenu();
            
        if(oldObtainedMoney < newObtainedMoney)
            updateMoney();
    }

    //activate the "press E to interact" text
    public void ActivateInteractionText()
    {
        screenText.text = "Click to interact";
    }
    //activate the "missing gadget" text
    public void ActivateNoGadgetText()
    {
        screenText.text = "You don't have the right gadget";
    }

    public void ActivateNoKeyText()
    {
        screenText.text = "The door is locked";
    }

    public void ActivateExitText()
    {
        screenText.text = "Click to Exit";
    }

    public void deactivateText()
    {
        screenText.text = "";
    }

    //add money to the total balance
    public void AddMoney(int money,string tag)
    {
        newObtainedMoney = newObtainedMoney +money;
        

        if (IsStealable(tag))
            obtainedObjects = obtainedObjects + 1;
    }

    //used when creating the gameOver menu, coounts all interactable objects
    private int getInteractableObjectsNumber(int layer)
    {
        int interactableObjectNumber = 0;
        //get all the objects from the scene
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        //filter all the object to only get the interactable ones
        List<GameObject> filtered = new List<GameObject>(goArray.Where(x => x.layer == layer));
        foreach (GameObject obj in filtered)
        {
            if (IsStealable(obj.tag))
            {
                interactableObjectNumber = interactableObjectNumber + 1;
            }
        }
        return interactableObjectNumber;
    }

    //used to check if an item is stealable
    private bool IsStealable(string itemTag) {
        foreach(string tag in notStealable) {
            if (itemTag == tag) return false;
        }
        return true;
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
