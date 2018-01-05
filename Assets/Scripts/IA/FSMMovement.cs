using UnityEngine;

public class FSMMovement {

    // check if player is in movement
    public bool isRunning;
    // check if player is spotted
    public bool isSpotted;
    // check if player is stuned
    public bool isStunned;
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
        if (isRunning && !isSpotted && !isStunned) {
			// get the shift
			direction = (position - lastPosition).normalized;
            // set position like the last
			lastPosition = position;
        }
		// getDirection
		GetDirection();
        //round direction to have only -1, 0 and 1 as possible directions
        direction = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        float newSpeed = speed;
        //remove diagonal directions (to avoid light position interpolation)
        if(Mathf.Abs(direction.x) > 0f && Mathf.Abs(direction.y) > 0f) {
            direction = new Vector2(0f, direction.y);
            // check if we move in diagonal on the map and it reduces the speed
            newSpeed = speed / Mathf.Sqrt(2f);
        }
		// set values of the parameters
		animator.SetFloat("RunX", direction.x);
		animator.SetFloat("RunY", direction.y);
		animator.SetBool("Running", isRunning);
		animator.SetBool("Spotting", isSpotted);
        animator.SetBool("Stunning", isStunned);
		// return the speed of the IA
        return newSpeed;
    }

	// get the direction of the transform given
	Vector2 GetDirection() {
        // top
        if (direction.y > 0f && direction.y > Mathf.Abs(direction.x)) {
            return lastDir = Vector2.up;
        }
        if (direction.x > 0f && direction.y <= Mathf.Abs(direction.x)) {
            return lastDir = Vector2.right;
        }
        if (direction.x < 0f && direction.y > Mathf.Abs(direction.x)) {
            return lastDir = Vector2.down;
        }
        if (direction.x < 0f && direction.y <= Mathf.Abs(direction.x)) {
            return lastDir = Vector2.left;
        }
        return lastDir;
    }
}
