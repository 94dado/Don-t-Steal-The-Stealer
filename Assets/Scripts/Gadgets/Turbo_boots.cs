using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbo_boots : Gadgets {

    public Turbo_boots(string name, Sprite sprite, float cooldown, float boostDuration, PlayerController player) : base(name,sprite,cooldown, boostDuration, player)
    {
       
    }

    override
	public void activateGadget()
    {
        if (cooldownTimer == 0)
        {
            player.speed = player.boostedSpeed;
            boostDurationTimer = boostDuration;
            cooldownTimer = cooldown;
        }
    }

    override
    public void deactivateGadget()
    {
        player.speed = 3;
    }
}
