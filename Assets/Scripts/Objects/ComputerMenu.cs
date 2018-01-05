using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerMenu : InteractableObject {
    public string scene;
    public string mainMenu;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ChangeScene(mainMenu);
        }
    }

    public override int Interact() {
        ChangeScene(scene);
        return 0;
    }

    private void ChangeScene(string name) {
        if(name != "") {
            SceneManager.LoadScene(name);
            Cursor.visible = true;
        }
    }
}
