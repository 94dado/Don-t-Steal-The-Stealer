using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GadgetBuyer : MonoBehaviour {

    public bool activeGadgetPanel;
    public bool activeIntelligencePanel;
    public  int gadgetID;
    public int intelligenceID;
    private DataManager dataManager;
    public Text description;
    private Button buyButton;

    private void Start()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        buyButton = GetComponent<Button>();
        gadgetID = -1;
        intelligenceID = -1;
        activeGadgetPanel = true;
        activeIntelligencePanel = false;
    }

    private void Update()
    {
        if ((gadgetID != -1 && buyButton.interactable == false )|| (intelligenceID != -1 && buyButton.interactable == false))
            buyButton.interactable = true;
    }

    public void buyGadget()
    {
        if(gadgetID != -1 && activeGadgetPanel && dataManager.Gadgets[gadgetID].isLocked && dataManager.MoneyData >= dataManager.Gadgets[gadgetID].price)
        {
            dataManager.MoneyData = dataManager.MoneyData - dataManager.Gadgets[gadgetID].price;
            dataManager.Gadgets[gadgetID].isLocked = false;
        }

        
        else if (intelligenceID != -1 && activeIntelligencePanel&& dataManager.Intelligence[intelligenceID].isLocked && dataManager.MoneyData >= dataManager.Intelligence[intelligenceID].price)
        {
            dataManager.MoneyData = dataManager.MoneyData - dataManager.Intelligence[intelligenceID].price;
            dataManager.Intelligence[intelligenceID].isLocked = false;
            dataManager.Intelligence[intelligenceID].Buy();
        }
    }

    public void setGadget(int gadgetID)
    {
        activeGadgetPanel = true;
        activeIntelligencePanel = false;
        this.gadgetID = gadgetID;
        
    }

    public void setIntelligence(int intelligenceID)
    {
        activeGadgetPanel = false;
        activeIntelligencePanel = true;
        this.intelligenceID = intelligenceID;
    }

    public int getGadget()
    {
        return this.gadgetID;
    }

    public int getIntelligence()
    {
        return this.intelligenceID;
    }

    
}
