using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

    public GameObject victoryMenu;
    public GameObject gameOverMenu;


    public Text gameOverTimeLimitText;
    public Text gameOverObjectsText;
    public Text gameOverObjectsCountText;
    public Text gameOverTimeCountText;
    private DataManager dataManager;

    public Text victoryTimeLimitText;
    public Text victoryObjectsText;
    public Text victoryObjectsCountText;
    public Text victoryTimeCountText;
    public GameObject objectStar;
    public GameObject timeStar;
    public GameObject firstStarPrize;
    public GameObject secondStarPrize;
    public GameObject thirdStarPrize;

    private bool gameOverActivated;
    private bool winActivated;

    public int timeLimit;
    public int level;

    private GameManager gameManager;
    MusicManager musicManager;

    //remove this variables
    [HideInInspector]
    public bool gameOver, win;

    // Use this for initialization
    void Start () {
        gameManager = GameManager.instance;
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        musicManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<MusicManager>();
        level = SceneManager.GetActiveScene().buildIndex - 2;
        firstStarPrize.GetComponent<Text>().text = "+" + dataManager.Levels[level].firstStarPrize + "$";
        secondStarPrize.GetComponent<Text>().text = "+" + dataManager.Levels[level].secondStarPrize + "$";
        thirdStarPrize.GetComponent<Text>().text = "+" + dataManager.Levels[level].thirdStarPrize + "$";
    }


    //call this method to activate victory menu
    public void activateVictoryMenu()
    {
        if (!winActivated)
        {
            bool firstStarAlreadyObtained = false;
            bool secondStarAlreadyObtained = false;
            bool thirdStarAlreadyObtained = false;

            if (dataManager.Levels[level].StarsScore >= 1)
                firstStarAlreadyObtained = true;
            if (dataManager.Levels[level].StarsScore >= 2)
                secondStarAlreadyObtained = true;
            if (dataManager.Levels[level].StarsScore >= 3)
                thirdStarAlreadyObtained = true;

            // play music of winning
            musicManager.PlaySound(musicManager.winningSongName);
            dataManager.Levels[level].levelCompleted = true;
            if(dataManager.Levels[level].objectsScore < gameManager.obtainedObjects)
                dataManager.Levels[level].objectsScore = gameManager.obtainedObjects;
            if(dataManager.Levels[level].timeScore < (int)gameManager.time)
                dataManager.Levels[level].timeScore = (int)gameManager.time;
            dataManager.MoneyData = dataManager.MoneyData + gameManager.newObtainedMoney;
            if (!firstStarAlreadyObtained)
            {
                dataManager.MoneyData = dataManager.MoneyData + dataManager.Levels[level].firstStarPrize;
                firstStarPrize.SetActive(true);
                
            }

            victoryTimeCountText.text = calculateTime((int)gameManager.time);
            victoryObjectsCountText.text = gameManager.obtainedObjects + "/" + gameManager.obtainableObjects;
            victoryTimeLimitText.text = "\nSteal all objects in less \nthan " + calculateTime(timeLimit);
            
            if (gameManager.obtainedObjects == gameManager.obtainableObjects)
            {
                victoryObjectsText.color = new Color(0, 1, 0, 1);
                victoryObjectsCountText.color = new Color(0, 1, 0, 1);
                objectStar.GetComponent<Image>().enabled = true;
                if(!secondStarAlreadyObtained)
                {
                    dataManager.MoneyData = dataManager.MoneyData + dataManager.Levels[level].secondStarPrize;
                    secondStarPrize.SetActive(true);
                }

                if (gameManager.time < timeLimit + 1)
                {
                    victoryTimeLimitText.color = new Color(0, 1, 0, 1);
                    victoryTimeCountText.color = new Color(0, 1, 0, 1);
                    timeStar.GetComponent<Image>().enabled = true;

                    if (!thirdStarAlreadyObtained)
                    {
                        dataManager.MoneyData = dataManager.MoneyData + dataManager.Levels[level].thirdStarPrize;
                        thirdStarPrize.SetActive(true);
                    }
                }
            }



            victoryMenu.SetActive(true);
            winActivated = true;
            
        }
    }

    //call this method to activate game over menu
    public void activateGameOverMenu()
    {
        if (!gameOverActivated)
        {
            // playe music of gameover song
            musicManager.PlaySound(musicManager.gameoverSongName);
            gameOverObjectsCountText.text = gameManager.obtainedObjects + "/" + gameManager.obtainableObjects;
            gameOverTimeCountText.text = calculateTime((int)gameManager.time);
            gameOverTimeLimitText.text = "\nSteal all objects in less \nthan " + calculateTime(timeLimit);

            if (gameManager.obtainedObjects == gameManager.obtainableObjects)
            {
                gameOverObjectsText.color = new Color(0, 1, 0, 1);
                gameOverObjectsCountText.color = new Color(0, 1, 0, 1);

                if (gameManager.time < timeLimit + 1)
                {
                    gameOverTimeLimitText.color = new Color(0, 1, 0, 1);
                    gameOverTimeCountText.color = new Color(0, 1, 0, 1);
                }
            }

            gameOverMenu.SetActive(true);
            gameOverActivated = true;
        }

    }


    //this method turns time in seconds into time in HH:MM:SS
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
