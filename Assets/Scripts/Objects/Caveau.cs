using UnityEngine;

public class Caveau : InteractableObject {

    void Start() {
        animator = GetComponent<Animator>();
    }

    public override int Interact() {
        animator.SetBool("interacted", true);
        gameObject.layer = LayerMask.NameToLayer("Interacted");

        return value;
    }
}
