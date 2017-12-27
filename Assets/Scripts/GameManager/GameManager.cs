using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    
    public Text screenText;
    
    public GameOverManager gameOverManager;
    public Text moneyText;
    public Text timeText;
    public Text itemText;
    public Text keyText;
    public Text coolDownText;
    public Image gadgetImage;
    public Text gadgetText;
    private PlayerController player;

    [HideInInspector]
    // check if we are in game over
    public bool gameOver;
    [HideInInspector]
    // check if we are win
    public bool win;

    //gadget variables
    [HideInInspector]
    public List<Gadgets> gadgetList;
    [HideInInspector]
    public int currentGadgetNumber;
    
    public Gadgets currentGadget;
    private int gadgetNumber;

    private bool activeCooldownText;

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
    [HideInInspector]
    public int keyNumber;
    [HideInInspector]
    public int obtainedKeys;

    //list of all interactable that aren't stealable
    public string[] notStealable;

    //map variables
    private bool mapActive;
    public GameObject map;


    public static GameManager instance;



    private void Awake()
    {
        instance = this;
        oldObtainedMoney = 0;
        newObtainedMoney = 0;
        obtainedObjects = 0;
        obtainableObjects = getInteractableObjectsNumber(LayerMask.NameToLayer("Interactable"));

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();        

        InitializeGadgets();

    }

    private void Update()
    {
        //keep increasing timer untill the game has finished
        if (!gameOver && !win)
        {
            showTimer();
            MapUpdate();
            ShowGadget();
            UpdateCoolDownText();
        }
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
    public void ActivateSafeText()
    {
        screenText.text = "You need an Electronic Safe Opener";
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
        else if (tag == "Key")
            obtainedKeys++;
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
            InteractableObject io = obj.GetComponent<InteractableObject>();
            if(io != null && io.getObjectType() == "Key")
            {
                keyNumber++;
            }
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

    //open and close map
    void MapUpdate()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!mapActive)
            {
                map.SetActive(true);
                mapActive = true;
            }
            else
            {
                map.SetActive(false);
                mapActive = false;
            }
        }

        itemText.text = "Items: " + obtainedObjects + "/" + obtainableObjects;
        keyText.text = "Keys: " + obtainedKeys + "/" + keyNumber;
    }

    void ShowGadget()
    {

        
    float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (!Input.GetMouseButton(1))
        {
            if (scroll > 0)
            {
                if (currentGadgetNumber == gadgetNumber - 1)
                    currentGadgetNumber = 0;
                else
                    currentGadgetNumber++;
            }
            if (scroll < 0)
            {
                if (currentGadgetNumber == 0)
                    currentGadgetNumber = gadgetNumber - 1;
                else
                    currentGadgetNumber--;
            }

            if (scroll != 0)
            {
                currentGadget = gadgetList[currentGadgetNumber];
                gadgetImage.sprite = currentGadget.sprite;
                gadgetText.text = currentGadget.name;
            }
        }
    }

    void UpdateCoolDownText()
    {
        if(currentGadget.cooldownTimer != 0)
        {
            gadgetImage.color = new Color(0.2f, 0.2f, 0.2f);
            coolDownText.text = Mathf.Ceil(currentGadget.cooldownTimer).ToString();
        }
        else
        {
            gadgetImage.color = new Color(1, 1, 1);
            coolDownText.text = "";
        }
    }

    void InitializeGadgets()
    {
        DataManager dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        gadgetList = new List<Gadgets>();
        Gadget[] allGadgets;
        allGadgets = dataManager.Gadgets;


        foreach (Gadget g in allGadgets)
        {
            if (!g.isLocked)
            {
                if (g.name == "Turbo Boots")
                    gadgetList.Add(new Turbo_boots(g.name, g.image, g.coolDown, g.boostDuration, player));
                else if (g.name == "Rock")
                    gadgetList.Add(new Rock(g.name, g.image, g.coolDown, g.boostDuration, player));
                else if (g.name == "Lock Pick")
                    gadgetList.Add(new Lock_pick(g.name, g.image, g.coolDown, g.boostDuration, player));
                else if (g.name == "Electronic Safe Opener")
                    gadgetList.Add(new Electronic_safe_opener(g.name, g.image, g.coolDown, g.boostDuration, player));
                else if (g.name == "Banana")
                    gadgetList.Add(new Banana(g.name, g.image, g.coolDown, g.boostDuration, player));
                else if (g.name == "Laser Visor")
                    gadgetList.Add(new Laser_visor(g.name, g.image, g.coolDown, g.boostDuration, player));
                else
                    gadgetList.Add(new Gadgets(g.name, g.image, g.coolDown, g.boostDuration, player));
            }
        }

        gadgetNumber = gadgetList.Count;
        currentGadget = gadgetList[0];

        gadgetImage.sprite = currentGadget.sprite;
        gadgetText.text = currentGadget.name;


    }

}
