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
        if(dataManager.MoneyData >= dataManager.Gadgets[gadgetID].price)
        {
            dataManager.MoneyData = dataManager.MoneyData - dataManager.Gadgets[gadgetID].price;
            dataManager.Gadgets[gadgetID].isLocked = false;
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
