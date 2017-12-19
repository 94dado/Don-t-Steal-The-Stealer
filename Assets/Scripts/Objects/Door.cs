using UnityEngine;

public class Door : InteractableObject {

    public int doorID;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    override
    public int Interact()
    {
        animator.SetBool("interacted", true);
        Collider2D[] colliders;
        colliders = this.gameObject.GetComponents<Collider2D>() as Collider2D[];
        colliders[0].enabled = false;
        colliders[1].enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Interacted");

        return value;
    }

    public int getDoorID()
    {
        return this.doorID;
    }
}
