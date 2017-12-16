using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock_pick : Gadgets {

    public Lock_pick(string name, Sprite sprite, float cooldown, float boostDuration, PlayerController player) : base(name, sprite, cooldown, boostDuration, player)
    {

    }

    override
    public void activateGadget()
    {
        if(player.interact == false && player.nearADoor == true)
        {
            InteractableObject myObject = player.hitObject.collider.gameObject.GetComponent<InteractableObject>();
            myObject.Interact();
        }
    }
}
