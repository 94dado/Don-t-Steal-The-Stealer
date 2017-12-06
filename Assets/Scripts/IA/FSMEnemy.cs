using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FSMEnemy : MonoBehaviour {

	// all th points
	public Transform allPoints;
	// uset to calculate the next position
	public int pos;
    // radious of overlap circle
    public float overlapRadius;
    // patrol time
    [Range(0.1f, 5f)] public float FSMDelay;
    // time to wait before move to the next point
    [Range(1f, 10f)] public float idleTime;
    // spped of enemy
    public float speed;
    // point mask
    public LayerMask pointsMask;
    // wall mask
    public LayerMask obstaclesMask;
    public LayerMask interactableMask;
	// collision wih player and iteractable object
	public LayerMask collisionMask;

    FSM fsmMachine;
    FSMMovement movement;
    Animator animator;
    // current speed;
    float currentSpeed;
	// point that enemy have to reach
	Transform[] points;
    Transform nextPosition;
    Transform throwablePosition;
    // used to came back to the first known point
    Queue<Transform> toThrowableAndBack = new Queue<Transform>();
	// last direction
	Vector2 lastDir;
    bool isIdle;
    bool isPositionReached;
    bool isReachingThrowable;
    bool throwableIsReached;

    void Start() {
        animator = GetComponent<Animator>();
        // initialize movement
        movement = new FSMMovement(animator, transform.position, speed);
		// create the points for the movement
		points = allPoints.GetComponentsInChildren<Transform>();
		points = points.Skip(1).ToArray();
        // initialize FSM
        StartFSM();
    }

    void Update() {
        // check collision with player or object
        CheckCollision();
		// check the movement boolean
		if (movement.isSpot) {
			// stop moving
			movement.isRunning = false;
			Time.timeScale = 0;
		} 
        // animate the player
        currentSpeed = movement.Move(transform.position);
        // check if we have to move
		if ((movement.isRunning || isReachingThrowable) && !movement.isSpot) {
            Moving();
        }
    }

    // move the player
    void Moving() {
        // move player to next position until it will reach it
        if (Vector2.Distance(transform.position, nextPosition.position) > 0f) {
            // move
            transform.position = Vector2.MoveTowards(transform.position, nextPosition.position, currentSpeed * Time.deltaTime);
        }
        else {
            // stop moving and came back to idle
			if (movement.isRunning) {
				movement.isRunning = false;
                isPositionReached = true;
            }
            else {
                // you are arrived
                if (nextPosition == throwablePosition) {
                    isReachingThrowable = false;
                    throwablePosition = null;
                    throwableIsReached = true;
                }
                else {
                    // find the next position near the throwable
                    FindPosition();
                }
            }
        }
    }

    // check enter collision
    void CheckCollision() {
        // get all the colliders in a certain radius
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius, collisionMask);
        for (int i = 0; i < colliders.Length; i++) {
            // check if player is visible
            if (colliders[i].transform.tag == "Player") {
                // check position
				movement.isSpot |= PlayerIsVisible(colliders[i].transform.position);
                //check if the match has to end
                if (movement.isSpot) GameManager.instance.gameOver = true;
            }
            if (colliders[i].transform.tag == "Throwable") {
                throwablePosition = colliders[i].transform;
            }
        }
    }

    // check if player is visible from the IA
    bool PlayerIsVisible(Vector3 playerPosition) {
		Vector2 playerDirection = (playerPosition - transform.position).normalized;
		Vector2 dir = GetDirection (movement.direction.x, movement.direction.y);
		float dot = Vector2.Dot (dir, playerDirection);
		if (dot > 0.9f) {
            // player found
			Debug.Log("Found by " + transform.name);
            return true;
        }
        return false;
    }

	// get the direction of the transform given
	Vector2 GetDirection(float x, float y) {
		// top
		if (x > 0f && y > 0f) {
			return lastDir = Vector2.up;
		}
		// up left
		else if (x < 0f && y > 0f) {
			return lastDir = Vector2.up;
		}
		// right
		else if (x > 0f && y == 0f) {
			return lastDir = Vector2.right;
		}
		// down
		else if (x < 0f && y < 0f) {
			return lastDir = Vector2.down;
		}
		// down right
		else if (x > 0f && y < 0f) {
			return lastDir = Vector2.down;
		}
		// left
		else if (x < 0f && y == 0f) {
			return lastDir = Vector2.left;
		} 
		else {
			return lastDir;
		}
	}

    void StartFSM() {

        // Define states and link actions when enter/exit/stay
        FSMState idleAction = new FSMState {
            enterActions = new FSMAction[] { WaitBeforeMove }
        };

        FSMState moveAction = new FSMState {
            enterActions = new FSMAction[] { Move }
        };

        FSMState seekAction = new FSMState {
            enterActions = new FSMAction[] { FindPosition }
        };

        FSMState catchAction = new FSMState {
            enterActions = new FSMAction[] { EndLevel }
        };

        // Define transitions
        FSMTransition fromIdleToMove = new FSMTransition(CheckTimeToMove);
        FSMTransition fromMoveToIdle = new FSMTransition(CheckRechedPosition);
        FSMTransition fromIdleToCatch = new FSMTransition(CheckPlayer);
        FSMTransition fromIdleToSeek = new FSMTransition(CheckNearObject);
        FSMTransition fromSeekToIdle = new FSMTransition(CheckReachedObject);
        FSMTransition fromSeekToCatch = new FSMTransition(CheckPlayer);
        FSMTransition fromMoveToSeek = new FSMTransition(CheckNearObject);
        FSMTransition fromMoveToCatch = new FSMTransition(CheckPlayer);

        // Link states with transitions
        idleAction.AddTransition(fromIdleToMove, moveAction);
        idleAction.AddTransition(fromIdleToCatch, catchAction);
        idleAction.AddTransition(fromIdleToSeek, seekAction);
        moveAction.AddTransition(fromMoveToIdle, idleAction);
        moveAction.AddTransition(fromMoveToSeek, seekAction);
        moveAction.AddTransition(fromMoveToCatch, catchAction);
        seekAction.AddTransition(fromSeekToIdle, idleAction);
        seekAction.AddTransition(fromSeekToCatch, catchAction);

        // Setup a FSA at initial state
        fsmMachine = new FSM(idleAction);
        // Start monitoring
        StartCoroutine("PatrolFSM");
    }

    // Periodic update, run forever
    IEnumerator PatrolFSM() {
        // if is not gameover or win
		while (!GameManager.instance.gameOver && !GameManager.instance.win) {
            fsmMachine.Update();
            yield return new WaitForSeconds(FSMDelay);
        }
    }

    // wait seconds before move
    void WaitBeforeMove() {
        StartCoroutine("WaitToMove");
    }

    IEnumerator WaitToMove() {
        yield return new WaitForSeconds(idleTime);
        // is ready to move to next point
        isIdle = false;
    }

    // move the player to next position
    void Move() {
        // it is came back to default path
        if (toThrowableAndBack.Count == 0) {
            // if it can't move thoward a door and is not at the last point
			if (!CheckDoor() && pos + 1 != points.Length) {
                // reverse array and came back
                pos = points.Length - pos -1;
                Array.Reverse(points);
            }
            pos++;
            nextPosition = points[pos];
            // if we re at the end of the array
            if (pos + 1 == points.Length) {
                // reverse array
                Array.Reverse(points);
                pos = 0;
            }
        }
        else {
            // came back to point path point by point
            nextPosition = toThrowableAndBack.Dequeue();
        }
		movement.isRunning = true;
    }

    // check if door is open or close
    bool CheckDoor() {
        Vector3 nextPos = points[pos + 1].position;
        // if the door is closed
		if (Physics2D.Raycast(transform.position, (nextPos - transform.position).normalized, Vector2.Distance(transform.position, nextPos), interactableMask)) {
			return false;
        }
        return true;
    }

    // find position to be reached
    void FindPosition() {
        Transform minBetweenThrowableAndPoints = GetMinBetweenThrowableAndPoints();
        // if there are no point the nearest is the throwable itself
        if (minBetweenThrowableAndPoints == null) {
            nextPosition = throwablePosition;
        }
        else {
            // // get all the point near throwable
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius, pointsMask);
            Transform min = null;
            for (int i = 0; i < colliders.Length; i++) {
                // check if a wall is not between the point and the enemy
                if (!Physics2D.Raycast(transform.position, colliders[i].transform.position, Vector2.Distance(transform.position, colliders[i].transform.position), obstaclesMask)) {
                    // get the min
                    if (min == null || (Vector2.Distance(minBetweenThrowableAndPoints.position, min.position) > Vector2.Distance(minBetweenThrowableAndPoints.position, colliders[i].transform.position))) {
                        min = colliders[i].transform;
                    }
                }
            }
            nextPosition = min;
        }
        // added position of point
        toThrowableAndBack.Enqueue(nextPosition);
        isReachingThrowable = true;
    }

    // return the min point near the throwable
    Transform GetMinBetweenThrowableAndPoints() {
        // get all the point near throwable
        Collider2D[] colliders = Physics2D.OverlapCircleAll(throwablePosition.position, overlapRadius, pointsMask);
        Transform min = null;
        for (int i = 0; i < colliders.Length; i++) {
            // check if a wall is not between the point and the throwable
            if (!Physics2D.Raycast(throwablePosition.position, colliders[i].transform.position, Vector2.Distance(throwablePosition.position, colliders[i].transform.position), obstaclesMask)) {
                // get the min
                if (min == null || (Vector2.Distance(throwablePosition.position, min.position) > Vector2.Distance(throwablePosition.position, colliders[i].transform.position))) {
                    min = colliders[i].transform;
                }
            }
        }
        return min;
    }

    // game over
    void EndLevel() {
        // start gameover
        GameManager.instance.gameOver = true;
    }

    // check if is time to switch to move
    bool CheckTimeToMove() {
        if (!isIdle) {
            return true;
        }
        return false;
    }

    // check if is time to switch to idle
    bool CheckRechedPosition() {
        if (isPositionReached) {
            isPositionReached = false;
            // idle until next time to move
            isIdle = true;
            return true;
        }
        return false;
    }

    // check if is time to switch to gameover
    bool CheckPlayer() {
		if (movement.isSpot) {
            return true;
        }
        return false;
    }

    // check if is time to reach to seek
    bool CheckNearObject() {
        if (throwablePosition != null) {
            isIdle = false;
			movement.isRunning = false;
            return true;
        }
        return false;
    }

    // check if is time to came back to idle
    bool CheckReachedObject() {
        if (throwableIsReached) {
            throwableIsReached = false;
            // came back to idle
            isIdle = true;
            return true;
        }
        return false;
    }
}
