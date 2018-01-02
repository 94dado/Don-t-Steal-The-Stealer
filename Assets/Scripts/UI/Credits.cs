using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Credits : MonoBehaviour {
    public Button backButton;
    Animator animator;
    SceneLoader sceneLoader;

    void Start() {
        animator = GetComponent<Animator>();
        if(backButton != null) {
            sceneLoader = backButton.GetComponent<SceneLoader>();
        }
        else {
            sceneLoader = GetComponent<SceneLoader>();
        }
    }

    // Update is called once per frame
    void Update () {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Close")) {
            sceneLoader.SwitchScene();
        }
	}
}
