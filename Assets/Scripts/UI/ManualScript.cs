using UnityEngine;
using UnityEngine.SceneManagement;

public class ManualScript : MonoBehaviour {

    public string sceneName;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Return)) {
            SceneManager.LoadScene(sceneName);
        }
	}
}
