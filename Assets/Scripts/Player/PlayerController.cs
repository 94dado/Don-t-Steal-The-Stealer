using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    // speed of the player
    public float speed;
    public float collisionOnXAxe;
    public float collisionOnYAxe;

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

    // move the player
    void Move() {
        // get the stress value of the axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // each frame, at the start player not run
        isRunning = false;
        // movemement not start directly when we press the horizontal movement key and if there is not an obstacle
        if (horizontal > 0f || horizontal < 0f) {
            transform.Translate(horizontal * speed * Time.deltaTime, 0f, 0f);
            isRunning = true;
            lastMovement = new Vector2(horizontal, 0f);
        }
        // movemement not start directly when we press the vertical movement key and if there is not an obstacle
        if (vertical > 0f || vertical < 0f) {
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