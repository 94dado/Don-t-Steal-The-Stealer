using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public int level;
    public string levelString;

	public void SwitchScene() {
        if (levelString != "") {
            SceneManager.LoadScene(levelString);
        }
        else {
            SceneManager.LoadScene(level);
        }
    }

    public void SetScene(int level)
    {
        this.level = level;
    }

    public void startNewGame()
    {
        DataManager dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        dataManager.MoneyData = 0;
        foreach(Gadget g in dataManager.Gadgets)
        {
            g.isLocked = true;
        }

        foreach(Level l in dataManager.Levels)
        {
            l.isLocked = true;
            l.levelCompleted = false;
            l.objectsScore = 0;
            l.timeScore = 0;

        }

        foreach(Intelligence i in dataManager.Intelligence)
        {
            i.isLocked = true;
        }

        dataManager.Levels[0].isLocked = false;

        SwitchScene();
        dataManager.AlreadyStarted = true;
    }

}
