using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GadgetBuyer : MonoBehaviour {

    public  int gadgetID;
    private DataManager dataManager;
    public Text description;

    private void Start()
    {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
    }

    public void buyGadget()
    {
        if(gadgetID < dataManager.Gadgets.Length && dataManager.MoneyData >= dataManager.Gadgets[gadgetID].price)
        {
            dataManager.MoneyData = dataManager.MoneyData - dataManager.Gadgets[gadgetID].price;
            dataManager.Gadgets[gadgetID].isLocked = false;
        }
        else if (dataManager.MoneyData >= dataManager.Intelligence[gadgetID - dataManager.Gadgets.Length].price && gadgetID >= dataManager.Gadgets.Length)
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
