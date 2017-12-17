using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : SpriteOffset {

    // these variables are used for interactions with interactable objects
    public float interactionRadius;
    // speed of the player
    public float speed;

    private Vector2 lineEndWest, lineEndEast, lineEndSouth, lineEndNorth;
    private Vector2 lineEnd, lineStart;
    private GameManager gameManager;

    private List<int> keyList;

    //variables user in player interaction with objects
    [HideInInspector]
    public RaycastHit2D hitObject;
    [HideInInspector]
    public bool interact = false;
    [HideInInspector]
    public bool nearADoor = false;
    [HideInInspector]
    public bool nearASafe = false;
    [HideInInspector]
    public bool isAiming = false;

    //boosted speed variables
    [SerializeField]
    public float boostedSpeed;
    
    Animator animator;
    // where the last movemement is
    Vector2 lastMovement;
    // checks if player is in idle
    bool isRunning;
    // used for diagonals
    float currentSpeed;
    Rigidbody2D myRigidbody;
    // aim controller for crosshair
    AimController aimController;
    // check if player want to remove the aim
    bool removeAim;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        // aim
        GameObject crosshair = GameObject.FindWithTag("Crosshair");
        if (crosshair != null) {
            aimController = crosshair.GetComponent<AimController>();
        }
        
        keyList = new List<int>();

        
        //"key" used to open already opened doors
        keyList.Add(-1);
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
            ActivateGadget();
            UpdateCooldowns();
            // aim controller
            AimInteraction();
		} 
		else {
			animator.SetBool ("Saw", true);
            Cursor.visible = true;
		}
    }

    // interact with crosshair if it exists
    void AimInteraction() {
        if (aimController != null) {
            // remove aim
            if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && !removeAim) {
                aimController.isActive = false;
                // activate crosshair
                aimController.DisableSprites();
                removeAim = true;
                isAiming = false;
            }
            // if player press aim appear
            if (Input.GetMouseButton(1) && isAiming && !removeAim) {
                aimController.isActive = true;
                // activate crosshair
                aimController.EnableSprites();
            }
            // if it release the button thrown an object
            if (Input.GetMouseButtonUp(1) && !removeAim && isAiming) {
                // deactivate crosshair
                isAiming = false;
                if (aimController.isThrowable) {
                    Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 point = new Vector3(mousePoint.x, mousePoint.y, 0);
                    gameManager.currentGadget.projectiles.getNextProjectile().Spawn(transform.position, point);
                    gameManager.currentGadget.deactivateGadget();
                }
                aimController.isActive = false;
                aimController.DisableSprites();
            }
            if (Input.GetMouseButtonUp(1) && removeAim) {
                removeAim = false;
            }
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
                lineEndEast.Set(gameObject.transform.position.x + interactionRadius / 2, gameObject.transform.position.y);
                lineEnd = lineEndEast;
            }
            else
            {
                lineEndWest.Set(gameObject.transform.position.x - interactionRadius / 2, gameObject.transform.position.y);
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
            bool hitWall;

            if (Physics2D.Linecast(lineStart, lineEnd, 1 << LayerMask.NameToLayer("Wall")))
                hitWall = true;
            else
                hitWall = false;
            

            float myObjectY = myObject.transform.position.y;
            float nancyY = transform.position.y;

            if(checkGadgetPresence(myObject) == -1)
            {
                gameManager.deactivateText();
            }

            else if (checkGadgetPresence(myObject) == 0)
            {
                if ((hitWall && nancyY < myObjectY) || (!hitWall))
                {
                    gameManager.ActivateInteractionText();
                    interact = true;
                }
                nearADoor = false;
                nearASafe = false;

            }
            else if (checkGadgetPresence(myObject) == 1)
            {
                gameManager.ActivateSafeText();
                nearADoor = false;
                nearASafe = true;
            }
            else if (checkGadgetPresence(myObject) == 2)
            {
                gameManager.ActivateNoKeyText();
                nearADoor = true;
                nearASafe = false;
            }
            else if (checkGadgetPresence(myObject) == 3)
            {
                gameManager.ActivateExitText();
                interact = true;
                nearADoor = false;
                nearASafe = false;
            }

        }

        else
        {
            interact = false;
            nearADoor = false;
            nearASafe = false;
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
        //-1 can't interact with this object
        //0 you can interact with the object
        //1 you need to use a safe opener
        //2 you are missing a key
        //3 you stole at least an object and are near an exit

        
        if (myObject.getObjectType() == "Rock" || myObject.getObjectType() == "Banana")
            return -1;

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
        else if (myObject.getObjectType() == "Safe")
        {
            return 1;
        }
        return 0;


    }

    private void ActivateGadget()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            gameManager.currentGadget.activateGadget();
        }
    }

    void UpdateCooldowns()
    {
        //update all the cooldown timers and duration timers
        for (int i = 0; i < gameManager.gadgetList.Count; i++)
        {
            gameManager.gadgetList[i].UpdateCooldown(Time.deltaTime);
        }

    }


}