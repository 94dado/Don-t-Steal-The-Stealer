using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    // speed of the player
    public float speed;

    Animator animator;
    // where the last movemement is
    Vector2 lastMovement;
    bool isRunning;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    // move the player
    void Move() {
        // get the speed on the axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // at the start player not run
        isRunning = false;
        // movemement not start directly when we press the horizontal movement key
        if (horizontal > 0.5f || horizontal < -0.5f) {
            transform.Translate(horizontal * speed * Time.deltaTime, 0f, 0f);
            isRunning = true;
            lastMovement = new Vector2(horizontal, 0f);
        }
        // movemement not start directly when we press the vertical movement key
        if (vertical > 0.5f || vertical < -0.5f) {
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