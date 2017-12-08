using UnityEngine;
using System;
using System.Linq;

public class FSMSpawnManager : MonoBehaviour {

	// guard object
	public GameObject guard;
	public Paths[] paths;

	// Use this for initialization
	void Awake () {
		Spawner();
	}

	void Spawner() {
		foreach(Paths item in paths) {
			// get the start position
			Transform[] allPoints = item.path.GetComponentsInChildren<Transform>().Skip(1).ToArray();
			GameObject newGuard = Instantiate(guard, allPoints[item.startPos].position, Quaternion.identity, transform);
			newGuard.GetComponent<FSMEnemy>().InitializeFSM(allPoints, item.startPos);
		}
	}

	[Serializable]
	// path and position of each guard
	public struct Paths {
		public Transform path;
		public int startPos;
	}
}
