using UnityEngine;

public class Atm : InteractableObject {

    public override int Interact() {
        //destroy the screen or the light
        Destroy(child);
        //change sprite if needed
        if(stolenSprite != null) {
            GetComponent<SpriteRenderer>().sprite = stolenSprite;
        }
        gameObject.layer = LayerMask.NameToLayer("Interacted");
        return value;
    }
}
