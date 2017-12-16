using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour {

    public LayerMask notTargetableMask;
    public LayerMask obstacleMask;
    public Transform player;
    public SpriteRenderer dot;
    public Color dotHighlightColor;
    [Range(50, 200)] public int aimRotationSpeed;

    [HideInInspector]
    public bool isActive;
    [HideInInspector]
    public bool isThrowable;

    Vector2 mousePoint;
    SpriteRenderer sprite;
    Color originalDotColor;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        // at start the game object is disabled
        DisableSprites();
        originalDotColor = dot.color;
	}
	
	// Update is called once per frame
	void Update () {
        Targeting();
	}

    // target the mouse
    void Targeting() {
        // little animation of rotation each frame
        transform.Rotate(Vector3.forward * -aimRotationSpeed * Time.deltaTime);
        // crosshairs point to the mouse cursor
        mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePoint.x, mousePoint.y, 0f);
        if (isActive) {
            DetectTarget();
        }
    }

    // select the target
    void DetectTarget() {
        RaycastHit2D hitPoint = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        // if we hit something
        if (hitPoint.collider == null || (hitPoint.collider != null && notTargetableMask != (notTargetableMask | (1 << hitPoint.collider.transform.gameObject.layer)))) {
            // if we hit something
            if (!Physics2D.Raycast(player.position, (mousePoint - new Vector2(player.position.x, player.position.y)).normalized, Vector2.Distance(mousePoint, player.position), obstacleMask)) {
                // show aim
                dot.color = dotHighlightColor;
                isThrowable = true;
            }
            else {
                // hide aim
                dot.color = originalDotColor;
                isThrowable = false;
            }
        }
        else {
            // hide aim
            dot.color = originalDotColor;
            isThrowable = false;
        }
    }

    // disable the aim sprite
    public void DisableSprites() {
        sprite.enabled = false;
        dot.enabled = false;
        isThrowable = false;
    }

    // enable the aim sprite
    public void EnableSprites() {
        sprite.enabled = true;
        dot.enabled = true;
        isThrowable = false;
    }
}
