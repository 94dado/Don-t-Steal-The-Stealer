using UnityEngine;

public class LayerBehaviour : MonoBehaviour {

    //parameters for LayerChanger
    public LayerMask interactionLayer;
    public Transform min;
    public Transform max;
    public int sensitivity = 3;

    //layer changer
    private LayerChanger lc;

    void Start() {
        lc = new LayerChanger(interactionLayer, min, max, sensitivity);
    }

    // Update is called once per frame
    void Update () {
        lc.UpdateOrder();   
	}

    //change the layer changer floor
    public void ChangeFloor(Transform min, Transform max) {
        lc.ChangeFloor(min, max);
    }
}
