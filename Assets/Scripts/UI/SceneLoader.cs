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

}
