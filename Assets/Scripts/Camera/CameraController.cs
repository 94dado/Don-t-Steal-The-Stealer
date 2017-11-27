using UnityEngine;

public class CameraController : MonoBehaviour {

    // player
    public Transform target;
    // bound diagonal positions
    public Transform maxBound;
    public Transform minBound;

    Camera cam;
    // get the camera's aspect ratio
    float camVertExtent;
    float camHorzExtent;
    // get bounds
    float leftBound;
    float rightBound;
    float bottomBound;
    float topBound;

    void Start() {
        cam = GetComponent<Camera>();

        // get the camera's aspect ratio
        camVertExtent = cam.orthographicSize;
        camHorzExtent = cam.aspect * camVertExtent;

        // get bounds
        leftBound = minBound.position.x + camHorzExtent;
        rightBound = maxBound.position.x - camHorzExtent;
        bottomBound = minBound.position.y + camVertExtent;
        topBound = maxBound.position.y - camVertExtent;
    }

    void Update() {
        // clamp camera to boundaries
        float camX = Mathf.Clamp(target.position.x, leftBound, rightBound);
        float camY = Mathf.Clamp(target.position.y, bottomBound, topBound);

        // move camera
        transform.position = new Vector3(camX, camY, transform.position.z);
    }
}
