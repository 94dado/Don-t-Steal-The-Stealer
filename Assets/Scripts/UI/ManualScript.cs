using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManualScript : MonoBehaviour {

    public string sceneName;
    public Text pressEnter;


    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Return)) {
            SceneManager.LoadSceneAsync(sceneName);
            pressEnter.gameObject.SetActive(false);
        }
    }

}
