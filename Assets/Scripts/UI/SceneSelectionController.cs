using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelectionController : MonoBehaviour {

    public GameObject startButtonGameObject;
    private Button startButton;
    private SceneLoader sceneLoader;
    public GameObject newsPaper;
    public Sprite newsPaperPage;
    public GameObject firstStar;
    public GameObject secondStar;
    public GameObject thirdStar;

   
   

    private DataManager dataManager;
    public int level;


    public Image firstStarText;
    public Image secondStarText;
    public Image thirdStarText;

    public Text firstText;
    public Text secondText;
    public Text thirdText;
    private Button thisButton;


    public void Start()
    {


        sceneLoader = startButtonGameObject.GetComponent<SceneLoader>();
        startButton = startButtonGameObject.GetComponent<Button>();
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        thisButton = gameObject.GetComponent<Button>();
        


        loadStars();
        
    }

    public void Update()
    {
        if(!dataManager.Levels[level-3].isLocked && thisButton.interactable == false)
        {
            thisButton.interactable = true;
        }
    }

    public void SwitchScene()
    {
        sceneLoader.SetScene(level);
        if(!startButton.IsInteractable())
            startButton.interactable = true;
        if (!newsPaper.activeSelf)
            newsPaper.SetActive(true);
        newsPaper.GetComponent<Image>().sprite = newsPaperPage;
        loadInformation();
    }

    private void loadStars()
    {
        

        if (dataManager.Levels[level - 3].StarsScore >= 1)
            firstStar.SetActive(true);
        if (dataManager.Levels[level - 3].StarsScore >= 2)
            secondStar.SetActive(true);
        if (dataManager.Levels[level - 3].StarsScore >= 3)
            thirdStar.SetActive(true);
    }

    private void loadInformation()
    {
        firstText.text = "Complete the mission";
        secondText.text = "Steal " + dataManager.Levels[level - 3].maxObject + " objects"  ;
        thirdText.text = "Steal all objects in " + calculateTime(dataManager.Levels[level - 3].timeLimit);

        if (dataManager.Levels[level - 3].StarsScore >= 1)
            firstStarText.enabled = true;
        else
            firstStarText.enabled = false;
        if (dataManager.Levels[level - 3].StarsScore >= 2)
            secondStarText.enabled = true;
        else
            secondStarText.enabled = false;
        if (dataManager.Levels[level - 3].StarsScore >= 3)
            thirdStarText.enabled = true;
        else
            thirdStarText.enabled = false;
    }

    private string calculateTime(int timeInSeconds)
    {
        int showedSeconds;
        int showedMinutes;
        int showedHours;

        //calculate time in hours:minutes:seconds
        int remainingTime = (int)Mathf.Floor(timeInSeconds);
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
            return showedMinutesText + ":" + showedSecondText;
        else
            return showedHoursText + ":" + showedMinutesText + ":" + showedSecondText;
    }


}
