using UnityEngine;

public class LightTrigger : MonoBehaviour {

	//turn on/off the lightning
    void SetLightining(bool value) {
        GetComponentInChildren<Light>().enabled = value;
    }
}
