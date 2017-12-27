using UnityEngine;

public class Stair : InteractableObject {

    private FloorManager floorManager;
    public int nextFloor;
	// Use this for initialization
	void Start () {
        floorManager = FindObjectOfType(typeof(FloorManager)) as FloorManager;
	}

    public override int Interact() {
        floorManager.ChangeFloor(nextFloor);
        return 0;
    }
}
