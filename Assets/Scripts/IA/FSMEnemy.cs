using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FSMEnemy : MonoBehaviour {

	// all th points
	public Transform allPoints;
	// field of view of the guard
	[Range(0f, 180f)] public float FOVAngle;
	// uset to calculate the next position
	public int pos;
    // radious of overlap circle
    public float overlapRadius;
    // patrol time
    [Range(0.01f, 1f)] public float FSMDelay;
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
    bool isIdle;
	bool isReachedPosition;
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
		// checks the movement boolean
		if (GameManager.instance.gameOver) {
			// stop moving
			movement.isRunning = false;
			movement.isSpot = true;
		} 
		else {
			// checks collision with player or object
			CheckCollision();
			// check if we have to move
			if (movement.isRunning || isReachingThrowable) {
				Moving();
			}
		}
		// animates the player
		currentSpeed = movement.Move(transform.position);
    }

	// checks collision with palyer
	void OnTriggerEnter(Collider other) {
		GameManager.instance.gameOver |= other.transform.tag == "Player";
	}

    // moves the player
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
				isReachedPosition = true;
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

    // checks enter collision
    void CheckCollision() {
        // get all the colliders in a certain radius
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius, collisionMask);
        for (int i = 0; i < colliders.Length; i++) {
            // check if player is visible
            if (colliders[i].transform.tag == "Player") {
                // check position
				PlayerIsVisible(colliders[i].transform.position);
            }
            if (colliders[i].transform.tag == "Throwable") {
                throwablePosition = colliders[i].transform;
            }
        }
    }

	// checks if player is visible from the IA
	void PlayerIsVisible(Vector3 playerPosition) {
		Vector2 playerDirection = (playerPosition - transform.position).normalized;
		// if guard facing player and there are no wall between them
		if (IsFacingPlayer(playerDirection) && !CheckLayerWithRaycast(transform.position, playerPosition, obstaclesMask, true)) {
			// game over
			GameManager.instance.gameOver = true;
		}
	}
		
	// checks the face to face between player and IA
	bool IsFacingPlayer(Vector2 direction) {
		// check the face to face with dot, convert the dot product value into a 180 degree representation and check the view angle
		if (((1 - Vector2.Dot(movement.lastDir, direction)) * 180f) <= Mathf.Min(FOVAngle,360f)) {
			return true;
		}
		return false;
	}

    void StartFSM() {

        // Define states and link actions when enter/exit/stay
        FSMState idleAction = new FSMState {
            enterActions = new FSMAction[] { Idle }
        };

        FSMState moveAction = new FSMState {
            enterActions = new FSMAction[] { Move }
        };

        FSMState seekAction = new FSMState {
            enterActions = new FSMAction[] { FindPosition }
        };

        // Define transitions
        FSMTransition fromIdleToMove = new FSMTransition(CheckTimeToMove);
        FSMTransition fromMoveToIdle = new FSMTransition(CheckRechedPosition);
        FSMTransition fromIdleToSeek = new FSMTransition(CheckNearObject);
        FSMTransition fromSeekToIdle = new FSMTransition(CheckReachedObject);
        FSMTransition fromMoveToSeek = new FSMTransition(CheckNearObject);

        // Link states with transitions
        idleAction.AddTransition(fromIdleToMove, moveAction);
        idleAction.AddTransition(fromIdleToSeek, seekAction);
        moveAction.AddTransition(fromMoveToIdle, idleAction);
        moveAction.AddTransition(fromMoveToSeek, seekAction);
        seekAction.AddTransition(fromSeekToIdle, idleAction);

        // Setup a FSA at initial state
        fsmMachine = new FSM(idleAction);
        // Start monitoring
        StartCoroutine("PatrolFSM");
    }

    // Periodically updates, runs forever
    IEnumerator PatrolFSM() {
        // if is not gameover or win
		while (!GameManager.instance.gameOver && !GameManager.instance.win) {
            fsmMachine.Update();
            yield return new WaitForSeconds(FSMDelay);
        }
    }

    // waits seconds before move
    void Idle() {
        StartCoroutine("WaitToMove");
    }

    IEnumerator WaitToMove() {
        yield return new WaitForSeconds(idleTime);
        // is ready to move to next point
        isIdle = false;
    }

    // moves the player to next position
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

    // checks if door is open or close
    bool CheckDoor() {
        Vector3 nextPos = points[pos + 1].position;
        // if the door is closed
		if (CheckLayerWithRaycast(transform.position, nextPos, interactableMask, true)) {
			return false;
        }
        return true;
    }

    // finds position to be reached
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
				if (!CheckLayerWithRaycast(transform.position, colliders[i].transform.position, obstaclesMask, false)) {
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

    // returns the min point near the throwable
    Transform GetMinBetweenThrowableAndPoints() {
        // get all the point near throwable
        Collider2D[] colliders = Physics2D.OverlapCircleAll(throwablePosition.position, overlapRadius, pointsMask);
        Transform min = null;
        for (int i = 0; i < colliders.Length; i++) {
            // check if a wall is not between the point and the throwable
			if (!CheckLayerWithRaycast(throwablePosition.position, colliders[i].transform.position, obstaclesMask, false)) {
                // get the min
                if (min == null || (Vector2.Distance(throwablePosition.position, min.position) > Vector2.Distance(throwablePosition.position, colliders[i].transform.position))) {
                    min = colliders[i].transform;
                }
            }
        }
        return min;
    }

    // checks if is time to switch to move
    bool CheckTimeToMove() {
        if (!isIdle) {
            return true;
        }
        return false;
    }

    // checks if is time to switch to idle
    bool CheckRechedPosition() {
		if (isReachedPosition) {
			isReachedPosition = false;
			return isIdle = true;
        }
        return false;
    }

    // checks if is time to reach to seek
    bool CheckNearObject() {
        if (throwablePosition != null) {
            isIdle = false;
			movement.isRunning = false;
            return true;
        }
        return false;
    }

    // checks if is time to came back to idle
    bool CheckReachedObject() {
        if (throwableIsReached) {
            throwableIsReached = false;
            // came back to idle
            isIdle = true;
            return true;
        }
        return false;
    }

	// throws a raycast between two point to check if layer is between them
	bool CheckLayerWithRaycast(Vector2 pointA, Vector2 pointB, LayerMask layerMask, bool revert) {
		if(Physics2D.Raycast(pointA, RevertRayDirection(pointA, pointB, revert), Vector2.Distance(pointA, pointB), layerMask)) {
			return true;
		}
		return false;
	}

	// revert the direction of the ray if needed
	Vector2 RevertRayDirection(Vector2 pointA, Vector2 pointB, bool revert) {
		if(revert) {
			return (pointB - pointA).normalized;
		}
		return (pointA - pointB).normalized;
	}
}
