using UnityEngine;

public class FSMMovement {

    // check if player is in movement
    public bool isRunning;
    // check if player is spot
    public bool isSpot;
	// direction of the IA movement
	public Vector2 direction;
	// last movement direction
	public Vector2 lastDir;

    readonly Animator animator;
    // used to give last position of movement
    Vector2 lastPosition;
    // current speed of the IA
    readonly float speed;

	const float diagonal = 0.2f;

	public FSMMovement(Animator animator, Vector2 position, float speed) {
        this.animator = animator;
        this.speed = speed;
        lastPosition = position;
		direction = new Vector2 ();
    }

    // move the IA using animator
	public float Move(Vector2 position) {
        // if IA is moving and player hasn't seen
        if (isRunning && !isSpot) {
			// get the shift
			direction = (position - lastPosition).normalized;
            // set position like the last
			lastPosition = position;
        }
		// getDirection
		GetDirection();
		// set the animator with new data
		SetAnimator();
		// check if we move in diagonal on the map and it reduces the speed
		if (Mathf.Abs (direction.x) > 0f && Mathf.Abs (direction.y) > 0f) {
			return speed / Mathf.Sqrt (2f);
		} 
		// return the speed of the IA
		return speed;
    }

    // set the animator of the IA
    public void SetAnimator() {
		// set values of the parameters
		animator.SetFloat("RunX", direction.x);
		animator.SetFloat("RunY", direction.y);
        animator.SetBool("Running", isRunning);
        animator.SetBool("Spotting", isSpot);
    }

	// get the direction of the transform given
	public Vector2 GetDirection() {
		// top
		if (direction.y > 0f && direction.y > Mathf.Abs(direction.x)) {
			return lastDir = Vector2.up;
		}
		// right
		else if (direction.x > 0f && direction.y <= Mathf.Abs(direction.x)) {
			return lastDir = Vector2.right;
		}
		// down
		else if (direction.x < 0f && direction.y > Mathf.Abs(direction.x)) {
			return lastDir = Vector2.down;
		}
		// left
		else if (direction.x < 0f && direction.y <= Mathf.Abs(direction.x)) {
			return lastDir = Vector2.left;
		} 
		else {
			return lastDir;
		}
	}
}
