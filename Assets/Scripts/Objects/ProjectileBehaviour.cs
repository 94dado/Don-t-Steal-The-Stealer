using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

    Vector3 startingPosition;
    Vector3 endPosition;
    private float speed;
    public float duration;
    private float despawnTimer;

	// Use this for initialization
	void Start () {
        speed = 5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
        }

        if (despawnTimer > 0)
        {
            despawnTimer -= Time.deltaTime;
        }

        if (despawnTimer < 0)
        {
            despawnTimer = 0;
            gameObject.SetActive(false);
        }
    }

    public void Spawn(Vector3 startPosition, Vector3 endPosition)
    {
        gameObject.SetActive(true);
        transform.position = startPosition;
        this.endPosition = endPosition;
        despawnTimer = duration;
    }
}
