using UnityEngine;

public class Caveau : Door {

    public override int Interact() {
        animator.SetBool("interacted", true);
        gameObject.layer = LayerMask.NameToLayer("Interacted");

        return value;
    }
}
