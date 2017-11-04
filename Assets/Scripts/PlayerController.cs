using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    // speed of the player
    public float speed = 10f;

    Animator animator;
    // At the start the player facing right and bottom
    bool facingRight = true;
    bool facingTop;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    // move the player
    void Move() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // if player is moving left or right
        if (Mathf.RoundToInt(horizontal) != 0) {
            // start the movement animation
            animator.SetFloat("Speed", Mathf.Abs(horizontal));
            // move the player on the x axes
            transform.Translate(horizontal * speed * Time.deltaTime, 0f, 0f);
            // with a certain direction of the sprite
            FlipX(horizontal);
        }
        else {
            // start the movement animation
            animator.SetFloat("Speed", Mathf.Abs(vertical));
            // move the player on the x axes
            transform.Translate(0f, 0f, vertical * speed * Time.deltaTime);
            // with a certain direction of the sprite
            FlipY(vertical);
        }
    }

    // flip the movement direction on horizontal axe
    void FlipX(float horizontal) {
        // if the movement od the player is in the opposite direction compered to where the character looks
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) {
            facingRight = !facingRight;
            // mirror the sprite renderer
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = !facingRight;
        }
    }

    // flip the movement direction on vertical axe
    void FlipY(float vertical) {
        // if the movement od the player is in the opposite direction compered to where the character looks
        if ((vertical > 0 && !facingTop) || (vertical < 0 && facingTop)) {
            facingTop = !facingTop;
            // mirror the sprite renderer
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipY = !facingTop;
        }
    }
}
