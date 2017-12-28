using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerMenu : InteractableObject {
    public string scene;

    public override int Interact() {
        if(scene != "") {
            SceneManager.LoadScene(scene);
            Cursor.visible = true;
        }
        return 0;
    }
}
