using UnityEngine;

public class FSMMovement {

    // check if player is in movement
    public bool isRunning;
    // check if player is spot
    public bool isSpot;
	// direction of the IA movement
	public Vector2 direction;

    readonly Animator animator;
    // used to give direction of movement
    Vector2 lastPosition;
    // where the last movemement is
    Vector2 lastMovement;
    // current speed of the IA
    readonly float speed;

    public FSMMovement(Animator animator, Vector2 position, float speed) {
        this.animator = animator;
        this.speed = speed;
        lastPosition = position;
        lastMovement = new Vector2();
		direction = new Vector2 ();
    }

    // move the IA using animator
    public float Move(Vector2 position) {
        // get the shift
        direction = (position - lastPosition).normalized;
        // if IA is moving and player hasn't seen
        if (isRunning && !isSpot) {
            // move horizontally
            if (Mathf.Abs(direction.x) > 0f) {
                lastMovement = new Vector2(direction.x, 0f);
            }
            // move vertically
            if (Mathf.Abs(direction.y) > 0f) {
                lastMovement = new Vector2(0f, direction.y);
            }
            // set position like the last
            lastPosition = position;
			// set the animator with new data
			SetAnimator();
            // check if we move in diagonal on the map and it reduces the speed
            if (Mathf.Abs(direction.x) > 0f && Mathf.Abs(direction.y) > 0f) {
                return speed / Mathf.Sqrt(2f);
            }
        }
        // return the speed of the IA
        return speed;
    }

    // set the animator of the IA
    void SetAnimator() {
        // set values of the parameters
        animator.SetFloat("RunX", direction.x);
        animator.SetFloat("RunY", direction.y);
        animator.SetFloat("LastRunX", lastMovement.x);
        animator.SetFloat("LastRunY", lastMovement.y);
        animator.SetBool("Running", isRunning);
        animator.SetBool("Spotting", isSpot);
    }
}
