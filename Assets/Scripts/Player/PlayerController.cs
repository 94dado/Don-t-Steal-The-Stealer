using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    // speed of the player
    public float speed;
    public Transform lineStart,lineEnd, lineEndWest,lineEndEast,lineEndSouth,LineEndNorth;
    public GameObject interactionText;
    RaycastHit2D hitObject;
    bool interact = false;

    Animator animator;
    // where the last movemement is
    Vector2 lastMovement;
    // checks if player is in idle
    bool isRunning;
    // used for diagonals
    float currentSpeed;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Move();
        Raycast();
    }

    // move the player
    void Move() {
        // get the stress value of the axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // each frame, at the start player not run
        isRunning = false;
        // move horizontally
        if (horizontal > 0f || horizontal < 0f) {
            transform.Translate(horizontal * currentSpeed * Time.deltaTime, 0f, 0f);
            isRunning = true;
            lastMovement = new Vector2(horizontal, 0f);
            if (horizontal > 0f)
                lineEnd = lineEndEast;
            else
                lineEnd = lineEndWest;
        }
        // move vertically
        if (vertical > 0f || vertical < 0f) {
            transform.Translate(0f, vertical * currentSpeed * Time.deltaTime, 0f);
            isRunning = true;
            lastMovement = new Vector2(0f, vertical);
            if (vertical > 0f)
                lineEnd = LineEndNorth;
            else
                lineEnd = lineEndSouth;
        }
        // check if we move in diagonal on the map and it reduces the speed
        if (Mathf.Abs(horizontal) > 0.5f && Mathf.Abs(vertical) > 0.5f) {
            currentSpeed = speed / Mathf.Sqrt(2f);
        }
        else {
            currentSpeed = speed;
        }
        // set values of the parameters
        animator.SetFloat("RunX", horizontal);
        animator.SetFloat("RunY", vertical);
        animator.SetFloat("LastRunX", lastMovement.x);
        animator.SetFloat("LastRunY", lastMovement.y);
        animator.SetBool("Running", isRunning);
    }

    void Raycast() {
        Debug.DrawLine(lineStart.position, lineEnd.position, Color.black);

        if (Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Interactable")))
        {
            hitObject = Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Interactable"));
            interact = true;
            interactionText.SetActive(true);
        }
        else
        {
            interact = false;
            interactionText.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E) && interact == true)
            hitObject.collider.gameObject.GetComponent<InteractableObject>().Interact();

    }

}