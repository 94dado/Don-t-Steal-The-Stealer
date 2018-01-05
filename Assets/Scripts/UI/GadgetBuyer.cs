using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GadgetBuyer : MonoBehaviour {

    public  int gadgetID;
    private DataManager dataManager;
    public Text description;
    private Button buyButton;

    private void Start()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        buyButton = GetComponent<Button>();
        gadgetID = -1;
    }

    private void Update()
    {
        if (gadgetID != -1 && buyButton.interactable == false)
            buyButton.interactable = true;
    }

    public void buyGadget()
    {
        if(gadgetID < dataManager.Gadgets.Length && dataManager.Gadgets[gadgetID].isLocked && dataManager.MoneyData >= dataManager.Gadgets[gadgetID].price)
        {
            dataManager.MoneyData = dataManager.MoneyData - dataManager.Gadgets[gadgetID].price;
            dataManager.Gadgets[gadgetID].isLocked = false;
        }
        else if (gadgetID >= dataManager.Gadgets.Length && dataManager.Intelligence[gadgetID - dataManager.Gadgets.Length].isLocked && dataManager.MoneyData >= dataManager.Intelligence[gadgetID - dataManager.Gadgets.Length].price)
        {
            dataManager.MoneyData = dataManager.MoneyData - dataManager.Intelligence[gadgetID - dataManager.Gadgets.Length].price;
            dataManager.Intelligence[gadgetID - dataManager.Gadgets.Length].isLocked = false;
            dataManager.Intelligence[gadgetID - dataManager.Gadgets.Length].Buy();
        }
    }

    public void setGadget(int gadgetID)
    {
        this.gadgetID = gadgetID;
        
    }

    public int getGadget()
    {
        return this.gadgetID;
    }
}
