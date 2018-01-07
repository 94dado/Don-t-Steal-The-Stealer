using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GadgetShopManager : MonoBehaviour {

    private Button[] gadgetButtons;
    private Button[] intelligenceButtons;
    private Image[] gadgetImages;
    private Image[] intelligenceImages;
    private Text[] nameAndPriceGadgets;
    private Text[] nameAndPriceIntelligence;
    private DataManager dataManager;
    public GameObject[] gadgets;
    public GameObject[] intelligence;
    public Text description;
    public GameObject buyButton;
    private GadgetBuyer gadgetBuyer;
    private int currentGadget = -1;
    private int currentIntelligence = -1;
    public Text yourMoney;
    private bool gadgetPanelActive;
    private bool intelligencePanelActive;



    // Use this for initialization
    void Start () {
        gadgetPanelActive = true;
        intelligencePanelActive = false;
        description.text = "";
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        gadgetButtons = new Button[dataManager.Gadgets.Length];
        gadgetImages = new Image[dataManager.Gadgets.Length];
        nameAndPriceGadgets = new Text[dataManager.Gadgets.Length];
        intelligenceButtons = new Button[dataManager.Intelligence.Length];
        intelligenceImages = new Image[dataManager.Intelligence.Length];
        nameAndPriceIntelligence = new Text[dataManager.Intelligence.Length];
        gadgetBuyer = buyButton.GetComponent<GadgetBuyer>();


        for (int i = 0; i < dataManager.Gadgets.Length; i++)
        {
            gadgets[i].SetActive(true);
            gadgetButtons[i] = gadgets[i].GetComponentInChildren<Button>();
            gadgetImages[i] = gadgets[i].transform.GetChild(0).GetComponent<Image>();
            nameAndPriceGadgets[i] = gadgets[i].GetComponentInChildren<Text>();
            gadgetImages[i].sprite = dataManager.Gadgets[i].image;
            nameAndPriceGadgets[i].text = dataManager.Gadgets[i].gadgetName + "\n" + dataManager.Gadgets[i].price + " $";
        }

        for (int i = 0; i < dataManager.Intelligence.Length; i++)
        {
            intelligence[i].SetActive(true);
            intelligenceButtons[i] = intelligence[i].GetComponentInChildren<Button>();
            intelligenceImages[i] = intelligence[i].transform.GetChild(0).GetComponent<Image>();
            nameAndPriceIntelligence[i] = intelligence[i].GetComponentInChildren<Text>();
            intelligenceImages[i].sprite = dataManager.Intelligence[i].image;
            nameAndPriceIntelligence[i].text = dataManager.Intelligence[i].ToString() + "\n" + dataManager.Intelligence[i].price + " $";
        }


    }
	
	// Update is called once per frame
	void Update () {
        

        for(int i = 0; i < dataManager.Gadgets.Length; i++)
        {
            if (!dataManager.Gadgets[i].isLocked && gadgetButtons[i].interactable == true)
            {
                gadgetButtons[i].interactable = false;
                gadgetImages[i].color = new Color(0.2f, 0.2f, 0.2f);
            }
        }

        for (int i = 0; i < dataManager.Intelligence.Length; i++)
        {
            if (!dataManager.Intelligence[i].isLocked && intelligenceButtons[i].interactable == true)
            {
                intelligenceButtons[i].interactable = false;
                intelligenceImages[i].color = new Color(0.2f, 0.2f, 0.2f);
            }
        }


            updateDescription();

        yourMoney.text = "Money: " + dataManager.MoneyData + "$";
    }

    private void updateDescription()
    {
        
            if (currentGadget != gadgetBuyer.getGadget() && gadgetPanelActive == true )
            {
                currentGadget = gadgetBuyer.getGadget();
            if (currentGadget != -1)
                description.text = dataManager.Gadgets[currentGadget].description;
            else
                description.text = "";


            }


        if (currentIntelligence != gadgetBuyer.getIntelligence() && intelligencePanelActive == true)
            {
                currentIntelligence = gadgetBuyer.getIntelligence();
            if (currentIntelligence != -1)
                description.text = dataManager.Intelligence[currentIntelligence].description;
            else
                description.text = "";
            }
         
    }

    public void switchPanels()
    {
        if(gadgetPanelActive)
        {
            gadgetBuyer.setIntelligence(-1);
            gadgetPanelActive = false;
            intelligencePanelActive = true;
            
        }
        else
        {
            gadgetBuyer.setGadget(-1);
            gadgetPanelActive = true;
            intelligencePanelActive = false;
        }
        description.text = "";
        
    }

    
}
