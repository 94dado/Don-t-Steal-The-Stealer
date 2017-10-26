using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    // speed of the player
    public float speed = 10f;

    Animator animator;
    // At the start the player facing right
    bool facingRight = true;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        // start the movement animation
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        // move the player on the x axes
        transform.Translate(horizontal * speed * Time.deltaTime, 0f, 0f);
        // with a certain direction
        Flip(horizontal);
	}

    // flip the movement direction
    void Flip(float horizontal) {
        // if the movement od the player is in the opposite direction compered to where the character looks
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) {
            facingRight = !facingRight;
            // mirror the sprite renderer
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = !facingRight;
        }
    }
}
