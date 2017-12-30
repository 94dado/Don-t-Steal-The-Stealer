using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FSMEnemy : SpriteOffset {

	// field of view of the guard
	[Range(0f, 179.99f)] public float FOVAngle;
    // radious of overlap circle for player
    public float overlapRadiusPlayer;
    // radious of overlap circle for throwable
    public float overlapRadiusThrowable;
    // patrol time
    [Range(0.01f, 1f)] public float FSMDelay;
    // time to wait before move to the next point
    [Range(1f, 10f)] public float idleTime;
    // distance cuz banana make to fall IA
    [Range(0.1f, 0.5f)] public float fallRange;
    // time banana stun IA
    [Range(1f, 10f)] public float bananaStunTime;
    // spped of enemy
    public float speed;
    // point mask
    public LayerMask pointsMask;
    // wall mask
    public LayerMask obstaclesMask;
    // collision with interactable
    public LayerMask throwableMask;
	// collision wih player
	public LayerMask playerMask;
    // collision wih door closed
    public LayerMask interactableMask;

    FSM fsmMachine;
    FSMMovement movement;
    Animator animator;
    // current speed;
    float currentSpeed;
	// point that enemy have to reach
	Transform[] points;
	// uset to calculate the next position
	int currentPos;
    Transform nextPosition;
    Transform throwablePosition;
    // used to came back to the first known point
    Queue<Transform> toThrowableAndBack = new Queue<Transform>();
	bool isIdle = true;
    bool isReachingThrowable;
    bool throwableIsReached;
	// start FSM
	bool isFSMStarted;

	// initialize the FSM of the guard with points to follow and the start position
	public void InitializeFSM(Transform[] allPoints, int startPos) {
        animator = GetComponent<Animator>();
        // initialize movement
		movement = new FSMMovement(animator, transform.position, speed);
		points = allPoints;
		currentPos = startPos;
        // initialize FSM
        StartFSM();
    }

    void Update() {
		if(isFSMStarted) {
			// checks the movement boolean
			if(GameManager.instance.gameOver) {
				// stop moving
				movement.isRunning = false;
				movement.isSpotted = true;
				isFSMStarted = false;
			}
			else {
                // check collisions with gadgets if one is not already setted
                if (throwablePosition == null) {
                    CheckGadgetAround();
                }
                // check player position
                PlayerIsVisible();
				// check if we have to move
				if(movement.isRunning || isReachingThrowable) {
					Moving();
				}
			}
			// animates the player
			currentSpeed = movement.Move(transform.position);
		}
    }

	// checks collision with player
	void OnCollisionEnter2D(Collision2D other) {
		GameManager.instance.gameOver |= other.transform.tag == "Player";
	}

    // moves the player
    void Moving() {
        // move player to next position until it will reach it
        if (Vector2.Distance(transform.position, nextPosition.position) > 0.01f) {
            // move
            transform.position = Vector2.MoveTowards(transform.position, nextPosition.position, currentSpeed * Time.deltaTime);
        }
        else {
            // stop moving and came back to idle
			if (movement.isRunning) {
				movement.isRunning = false;
				isIdle = true;
            }
            else {
                // you are arrived
                if (nextPosition == throwablePosition) {
                    isReachingThrowable = false;
                    // remove the throwable from scene
                    Destroy(throwablePosition.gameObject);
                    throwablePosition = null;
                    throwableIsReached = true;
                    movement.isRunning = false;
                    // remove the position of the gadget destroyed
                    toThrowableAndBack.Dequeue();
                }
                else {
                    // find the next position near the throwable
                    FindPosition();
                }
            }
        }
    }

	// checks if player is visible from the IA
	void PlayerIsVisible() {
        // check the player
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, overlapRadiusPlayer, playerMask);
        if (playerCollider != null) {
            Vector2 playerDirection = (playerCollider.transform.position - transform.position).normalized;
            // if guard facing player and there are no wall between them
            if (IsFacingPlayer(playerDirection) && !CheckLayerWithRaycast(transform.position, playerCollider.transform.position, obstaclesMask, true)) {
                // game over
                GameManager.instance.gameOver = true;
            }
        }
	}

    // check which gadget the IA has in its radius
    void CheckGadgetAround() {
        // get all the colliders in a certain radius with layer throwable
        Collider2D[] throwableColliders = Physics2D.OverlapCircleAll(transform.position, overlapRadiusThrowable, throwableMask);
        Dictionary<float, Transform> bananas = new Dictionary<float, Transform>();
        Dictionary<float, Transform> rocks = new Dictionary<float, Transform>();
        // list each throwable
        for (int i = 0; i < throwableColliders.Length; i++) {
            // rock
            if (throwableColliders[i].transform.tag == "RockPool") {
                rocks.Add(Vector2.Distance(transform.position, throwableColliders[i].transform.position), throwableColliders[i].transform);
            }
            // banana
            else if (throwableColliders[i].transform.tag == "BananaPool") {
                bananas.Add(Vector2.Distance(transform.position, throwableColliders[i].transform.position), throwableColliders[i].transform);
            }
        }
        // banana behaviour
        if (bananas.Count > 0) {
            foreach (KeyValuePair<float, Transform> pair in bananas) {
                if (pair.Key <= fallRange) {
                    // IA stunned
                    StartCoroutine(StunWait(pair.Value));
                }
            }
        }
        // rocks behaviour
        if (rocks.Count > 0) {
            // get the first order for distance from IA
            if (rocks.Count > 1) {
                List<float> distances = rocks.Keys.ToList();
                distances.Sort();
                throwablePosition = rocks[distances.First()];
            }
            // get the first
            else {
                throwablePosition = rocks.First().Value;
            }
        }
    }

    // checks the face to face between player and IA
    bool IsFacingPlayer(Vector2 direction) {
		// check the face to face with dot, convert the dot product value into a 180 degree representation and check the view angle
		if (((1 - Vector2.Dot(movement.lastDir, direction)) * 180f) <= Mathf.Min(FOVAngle,359.99f)) {
			return true;
		}
		return false;
	}

    // IA stuns after contact with banana
    IEnumerator StunWait(Transform stunObject) {
        isFSMStarted = false;
        // remove banana
        Destroy(stunObject.gameObject);
        // active animation
        movement.isStunned = true;
        movement.isRunning = false;
        movement.Move(transform.position);
        yield return new WaitForSeconds(bananaStunTime);
        // IA restart to move
        movement.isStunned = false;
        movement.isRunning = true;
        isFSMStarted = true;
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
		isFSMStarted = true;
        // Start monitoring
        StartCoroutine("PatrolFSM");
    }

    // Periodically updates, runs forever
    IEnumerator PatrolFSM() {
        // if is not gameover or win
		while (isFSMStarted) {
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
			if (currentPos < points.Length -1 && !IsDoorClosed()) {
                // reverse array and came back
                currentPos = points.Length - currentPos -1;
                Array.Reverse(points);
            }
            // if we re at the end of the array
            if (currentPos >= points.Length -1) {
                // reverse array
                Array.Reverse(points);
                currentPos = 0;
            }
			currentPos++;
			nextPosition = points[currentPos];
        }
        else {
            // came back to point path point by point
            nextPosition = toThrowableAndBack.Dequeue();
        }
		movement.isRunning = true;
    }

    // checks if door is open or close
    bool IsDoorClosed() {
        // if the door is closed
		if (CheckLayerWithRaycast(transform.position, points[currentPos + 1].position, interactableMask, true)) {
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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadiusThrowable, pointsMask);
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(throwablePosition.position, overlapRadiusThrowable, pointsMask);
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
		if (isIdle) {
			return true;
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
			return isIdle = true;
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
