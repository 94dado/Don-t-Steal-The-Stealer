using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public int level;

	public void SwitchScene() {
        SceneManager.LoadScene(level);
    }

    public void SetScene(int level)
    {
        this.level = level;
    }

}
