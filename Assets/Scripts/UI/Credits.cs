using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Credits : MonoBehaviour {
    public Button backButton;
    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Close")) {
            backButton.GetComponent<SceneLoader>().SwitchScene();
        }
	}
}
