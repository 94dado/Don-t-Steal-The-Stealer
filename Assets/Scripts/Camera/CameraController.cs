using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public Transform target;

    void Update() {
        // move camera
        transform.position = target.position;
    }
}
