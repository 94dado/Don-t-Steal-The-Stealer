using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour {

    public LayerMask notTargetableMask;
    public LayerMask obstacleMask;
    public Transform player;
    [HideInInspector]
    public bool isThrowable;

    Color originalColor;
    Camera viewCamera;
    SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        viewCamera = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        // at start the game object is disabled
        transform.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        Targeting();
	}

    // target the mouse
    void Targeting() {
        // little animation of rotation each frame
        transform.Rotate(Vector2.right * -40 * Time.deltaTime);
        // crosshairs point to the mouse cursor
        transform.position = Input.mousePosition;
        DetectTarget();
    }

    // select the target
    void DetectTarget() {
        RaycastHit2D hitPoint = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        // if we hit something
        if (hitPoint.collider == null || (hitPoint.collider != null && notTargetableMask != (notTargetableMask | (1 << hitPoint.collider.transform.gameObject.layer)))) {
            // if we hit something
            if (!Physics2D.Raycast(transform.position, (transform.position - player.position).normalized, Vector2.Distance(transform.position, player.position), obstacleMask)) {
                // show aim
                sprite.color = originalColor;
                isThrowable = true;
            }
            else {
                // hide aim
                sprite.color = new Color(0f, 0f, 0f, 0f);
                isThrowable = false;
            }
        }
        else {
            // hide aim
            sprite.color = new Color(0f, 0f, 0f, 0f);
            isThrowable = false;
        }
    }
}
