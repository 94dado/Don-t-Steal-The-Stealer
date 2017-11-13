using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelectionController : MonoBehaviour {

    public GameObject startButtonGameObject;
    private Button startButton;
    private SceneLoader sceneLoader;

    public void Start()
    {
        sceneLoader = startButtonGameObject.GetComponent<SceneLoader>();
        startButton = startButtonGameObject.GetComponent<Button>();
    }

    public void SwitchScene(int level)
    {
        sceneLoader.SetScene(level);
        if(!startButton.IsInteractable())
            startButton.interactable = true;
    }
}
