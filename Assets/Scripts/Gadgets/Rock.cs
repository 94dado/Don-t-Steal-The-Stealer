using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Gadgets {

    public Rock(string name, Sprite sprite, float cooldown, float boostDuration, PlayerController player) : base(name, sprite, cooldown, boostDuration, player)
    {

    }

    override
    public void activateGadget()
    {
        if (cooldownTimer == 0)
        {
            boostDurationTimer = boostDuration;
            cooldownTimer = cooldown;
        }
    }


}
