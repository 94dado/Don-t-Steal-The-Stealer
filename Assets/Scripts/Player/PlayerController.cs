using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    // these variables are used for interactions with interactable objects
    public int interactionRadius;
    private Vector2 lineEndWest, lineEndEast, lineEndSouth, lineEndNorth;
    private Vector2 lineEnd, lineStart;
    private GameObject gameManagerObject;
    private GameManager gameManager;
    private List<string> gadgetList;
    RaycastHit2D hitObject;
    bool interact = false;

    // speed of the player
    public float speed;

    Animator animator;
    // where the last movemement is
    Vector2 lastMovement;
    // checks if player is in idle
    bool isRunning;
    // used for diagonals
    float currentSpeed;
    Rigidbody2D myRigidbody;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        gameManagerObject = GameObject.FindWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();

        //initializing gadgetList, this is temporary!!!!
        gadgetList = new List<string>();
        gadgetList.Add("Theca");
        gadgetList.Add("Painting");
        gadgetList.Add("Safe");
        gadgetList.Add("Door");

        //initializing player raycast
        lineEndWest = new Vector2(gameObject.transform.position.x - interactionRadius, gameObject.transform.position.y) ;
        lineEndEast = new Vector2(gameObject.transform.position.x + interactionRadius, gameObject.transform.position.y);
        lineEndSouth = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - interactionRadius);
        lineEndNorth = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + interactionRadius);
        lineEnd = lineEndSouth;
        lineStart = gameObject.transform.position;
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
            myRigidbody.velocity = new Vector2(horizontal * currentSpeed * Time.deltaTime, 0f);
            isRunning = true;
            lastMovement = new Vector2(horizontal, 0f);
            if (horizontal > 0f)
            {
                lineEndEast.Set(gameObject.transform.position.x + interactionRadius, gameObject.transform.position.y);
                lineEnd = lineEndEast;
            }
            else
            {
                lineEndWest.Set(gameObject.transform.position.x - interactionRadius, gameObject.transform.position.y);
                lineEnd = lineEndWest;
            }
        }
        // move vertically
        if (vertical > 0f || vertical < 0f) {
            myRigidbody.velocity = new Vector2(0f, vertical * currentSpeed * Time.deltaTime);
            isRunning = true;
            lastMovement = new Vector2(0f, vertical);
            if (vertical > 0f)
            {
                lineEndNorth.Set(gameObject.transform.position.x, gameObject.transform.position.y + interactionRadius);
                lineEnd = lineEndNorth;
            }
            else
            {
                lineEndSouth.Set(gameObject.transform.position.x, gameObject.transform.position.y - interactionRadius);
                lineEnd = lineEndSouth;
            }
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

    //cast a ray from the player to see if he can activate an interaction with an object
    void Raycast() {
        lineStart = gameObject.transform.position;
        Debug.DrawLine(lineStart, lineEnd, Color.black);

        if (Physics2D.Linecast(lineStart, lineEnd, 1 << LayerMask.NameToLayer("Interactable")))
        {
            hitObject = Physics2D.Linecast(lineStart, lineEnd, 1 << LayerMask.NameToLayer("Interactable"));
            if (checkGadgetPresence(hitObject.collider.gameObject.GetComponent<InteractableObject>().getObjectType()))
            {
                gameManager.ActivateInteractionText(true);
                interact = true;
            }
            else
                gameManager.ActivateNoGadgetText(true);
        }
        else
        {
            interact = false;
            gameManager.ActivateInteractionText(false);
            gameManager.ActivateNoGadgetText(false);
        }

        //if the user can interact with the object AND he presses E, he interacts
        if (Input.GetKeyDown(KeyCode.E) && interact == true)
        {
             gameManager.AddMoney(hitObject.collider.gameObject.GetComponent<InteractableObject>().Interact());
        }

    }

    private bool checkGadgetPresence(string objectType)
    {
        foreach(string gadget in gadgetList)
        {
            if (objectType == gadget)
                return true;
        }
        return false;

    }

}