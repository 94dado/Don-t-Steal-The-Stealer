using UnityEngine;

[RequireComponent(typeof(LayerBehaviour))]
public class FloorManager : MonoBehaviour {
    //main camera of the level
    public CameraController cam;
    //Nancy
    public Transform player;
    //list of all floors
    public Transform[] floors;
    //list of min/max bounds for each floor
    public Transform[] cameraLimits;
    //list of all spawn points for Nancy
    public Transform[] nancySpawns;

    //layer behaviour script
    LayerBehaviour layerHandler;
    public void Start() {
        layerHandler = GetComponent<LayerBehaviour>();
    }

    public void ChangeFloor(int floor) {
        //move Nancy to the second floor
        player.transform.position = nancySpawns[floor].position;
        //update camera limits
        Transform min = cameraLimits[floor].GetChild(0);
        Transform max = cameraLimits[floor].GetChild(1);
        cam.minBound = min;
        cam.maxBound = max;
        //update layer behaviour
        layerHandler.ChangeFloor(min, max);
    }
}
