using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : MonoBehaviour {

    public Transform nancy, guard;
    public Transform[] nancyPoints, guardPoints;
    bool isMoving = false;

	// Use this for initialization
	void Start () {
        nancy.position = nancyPoints[0].position;
        guard.position = guardPoints[0].position;
    }

    void Update() {
        if(!isMoving && Input.GetKeyUp(KeyCode.Return)) {
            isMoving = true;
            nancy.GetComponent<FSMEnemy>().InitializeFSM(nancyPoints, 0);
            guard.GetComponent<FSMEnemy>().InitializeFSM(guardPoints, 0);

        }
    }
}
