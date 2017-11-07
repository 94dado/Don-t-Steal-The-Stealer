using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    // speed of the player
    public float speed;
    [Range(0,1)] public float delay;
    public float stopDistanceXFromObstacles;
    public float stopDistanceYFromObstacles;
    public Transform baricenter;
    public LayerMask obstaclesMask;

    Animator animator;
    // where the last movemement is
    Vector2 lastMovement;
    bool isRunning;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Move();
    }

    // check if player is colliding vertically
    bool CheckCollisionVertically(float vertical) {
        // check distance from collision
        RaycastHit2D ray = Physics2D.Raycast(baricenter.position, Vector2.up * vertical, stopDistanceYFromObstacles, obstaclesMask);
        Debug.DrawLine(baricenter.position, ray.point, Color.blue, 3f);
        if (Physics2D.Raycast(baricenter.position, Vector2.up * vertical, stopDistanceYFromObstacles, obstaclesMask)) {
            Debug.DrawRay(baricenter.position, ray.point, Color.red, 3f);
            return true;
        }
        return false;
    }

    // check if player is colliding horizontally
    bool CheckCollisionHorizontally(float horizontal) {
        // check distance from collision
        if (Physics2D.Raycast(baricenter.position, Vector2.right * horizontal, stopDistanceXFromObstacles, obstaclesMask)) {
            return true;
        }
        return false;
    }

    // move the player
    void Move() {
        // get the stress value of the axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // each frame, at the start player not run
        isRunning = false;
        // movemement not start directly when we press the horizontal movement key and if there is an obstacle
        if ((horizontal > delay || horizontal < -delay) && !CheckCollisionHorizontally(horizontal)) {
            transform.Translate(horizontal * speed * Time.deltaTime, 0f, 0f);
            isRunning = true;
            lastMovement = new Vector2(horizontal, 0f);
        }
        // movemement not start directly when we press the vertical movement key and if there is an obstacle
        if ((vertical > delay || vertical < -delay) && !CheckCollisionVertically(vertical)) {
            transform.Translate(0f, vertical * speed * Time.deltaTime, 0f);
            isRunning = true;
            lastMovement = new Vector2(0f, vertical);
        }
        // set values of the parameters
        animator.SetFloat("RunX", horizontal);
        animator.SetFloat("RunY", vertical);
        animator.SetFloat("LastRunX", lastMovement.x);
        animator.SetFloat("LastRunY", lastMovement.y);
        animator.SetBool("Running", isRunning);
    }
}