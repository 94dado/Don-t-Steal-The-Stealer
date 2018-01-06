using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electronic_safe_opener : Gadgets {

    public Electronic_safe_opener(string name, Sprite sprite, float cooldown, float boostDuration, PlayerController player) : base(name, sprite, cooldown, boostDuration, player)
    {

    }

    override
    public void activateGadget()
    {
        if (player.interact == false && player.nearASafe == true)
        {
            InteractableObject myObject = player.hitObject.collider.gameObject.GetComponent<InteractableObject>();
            GameManager gameManager = GameManager.instance;
            gameManager.AddMoney(myObject.Interact(), myObject.getObjectType());
        }
    }

}
