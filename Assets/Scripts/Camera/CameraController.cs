using UnityEngine;

public class CameraController : MonoBehaviour {

    // player
    public Transform target;
    // bound diagonal positions
    public Transform maxBound;
    public Transform minBound;

    Camera cam;

    void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        // get the camera's aspect ratio
        float camVertExtent = cam.orthographicSize;
        float camHorzExtent = cam.aspect * camVertExtent;

        // get bounds
        float leftBound = minBound.position.x + camHorzExtent;
        float rightBound = maxBound.position.x - camHorzExtent;
        float bottomBound = minBound.position.y + camVertExtent;
        float topBound = maxBound.position.y - camVertExtent;

        // clamp camera to boundaries
        float camX = Mathf.Clamp(target.position.x, leftBound, rightBound);
        float camY = Mathf.Clamp(target.position.y, bottomBound, topBound);

        // move camera
        transform.position = new Vector3(camX, camY, transform.position.z);
    }
}
