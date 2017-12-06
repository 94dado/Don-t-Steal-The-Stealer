using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    // these variables are used for interactions with interactable objects
    public float interactionRadius;
    private Vector2 lineEndWest, lineEndEast, lineEndSouth, lineEndNorth;
    private Vector2 lineEnd, lineStart;
    private GameObject gameManagerObject;
    private GameManager gameManager;
    private List<string> gadgetList;

    private List<int> keyList;

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

        
        gadgetList = new List<string>();
        keyList = new List<int>();

        //initializing gadgetList and keyList, this is temporary!!!!
        
        gadgetList.Add("Theca");
        gadgetList.Add("Painting");
        gadgetList.Add("Safe");
        gadgetList.Add("Door");
        gameManager = GameManager.instance;

        //initializing player raycast
        lineEndWest = new Vector2(gameObject.transform.position.x - interactionRadius, gameObject.transform.position.y) ;
        lineEndEast = new Vector2(gameObject.transform.position.x + interactionRadius, gameObject.transform.position.y);
        lineEndSouth = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - interactionRadius);
        lineEndNorth = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + interactionRadius);
        lineEnd = lineEndSouth;
        lineStart = gameObject.transform.position;

        //hide cursor
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        // check game over
		if (!gameManager.gameOver && !gameManager.win) {
			Move ();
			Raycast ();
		} 
		else {
			animator.SetBool ("Saw", true);
            Cursor.visible = true;
            GetComponent<AudioSource>().Stop();
		}
    }

    // move the player
    void Move() {
        // get the stress value of the axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // each frame, at the start player not run
        isRunning = false;
        // move horizontally
        if (Mathf.Abs(horizontal) > 0f) {
			myRigidbody.MovePosition(new Vector2(transform.position.x + horizontal * speed * Time.deltaTime, transform.position.y + vertical * currentSpeed * Time.deltaTime));
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
        if (Mathf.Abs(vertical) > 0f) {
			myRigidbody.MovePosition(new Vector2(transform.position.x + horizontal * speed * Time.deltaTime, transform.position.y + vertical * currentSpeed * Time.deltaTime));
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
		animator.SetBool ("Saw", false);
    }

    //cast a ray from the player to see if he can activate an interaction with an object
    void Raycast()
    {

        lineStart = gameObject.transform.position;
        Debug.DrawLine(lineStart, lineEnd, Color.black);

        if (Physics2D.Linecast(lineStart, lineEnd, 1 << LayerMask.NameToLayer("Interactable")))
        {
            hitObject = Physics2D.Linecast(lineStart, lineEnd, 1 << LayerMask.NameToLayer("Interactable"));
            InteractableObject myObject = hitObject.collider.gameObject.GetComponent<InteractableObject>();
            if (checkGadgetPresence(myObject) == 0)
            {
                gameManager.ActivateInteractionText();
                interact = true;
            }
            else if (checkGadgetPresence(myObject) == 1)
                gameManager.ActivateNoGadgetText();
            else if (checkGadgetPresence(myObject) == 2)
                gameManager.ActivateNoKeyText();
            else if (checkGadgetPresence(myObject) == 3)
            {
                gameManager.ActivateExitText();
                interact = true;
            }

        }

        else
        {
            interact = false;
            gameManager.deactivateText();

        }

        //if the user can interact with the object AND he presses E, he interacts
        if (Input.GetKeyDown(KeyCode.Mouse0) && interact == true)
        {
            InteractableObject myObject = hitObject.collider.gameObject.GetComponent<InteractableObject>();
            if (myObject.getObjectType() != "Endgame")
            {
                gameManager.AddMoney(myObject.Interact(), myObject.tag);
            }

            else
            {
                gameManager.deactivateText();
                gameManager.win = true;
            }

            if (myObject.getObjectType() == "Key")
            {
                keyList.Add(((Key)myObject).getKeyID());
            }
        }

    }

    private int checkGadgetPresence(InteractableObject myObject)
    {
        //0 you can interact with the object
        //1 you are missing a gadget
        //2 you are missing a key
        //3 you still need to steal an object to exit

        // if the object is a door, check that the player has the key if he does return 0, else return 1
        if (myObject.getObjectType() == "Door")
        {
            foreach (int keyId in keyList)
            {
                if (((Door)myObject).getDoorID() == keyId)
                    return 0;
            }
            return 2;
        }
        // if the object is the exit mat and the player still needs to steal at least an item, return 3
        else if (myObject.getObjectType() == "Endgame" && gameManager.newObtainedMoney == 0)
            return 4;
        // if the object is a key or the mat (in this case the player has stolen at least one object) return 0
        else if (myObject.getObjectType() == "Key")
            return 0;
        else if (myObject.getObjectType() == "Endgame")
            return 3;
        //if we are here, the interacted object is neither a door, a key or a mat. We must check if the player has the right gadget
        else
        {
            foreach (string gadget in gadgetList)
            {
                if (myObject.getObjectType() == gadget)
                    return 0;
            }
            return 1;
        }



    }

}