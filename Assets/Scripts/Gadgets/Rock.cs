using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Gadgets {

    

    public Rock(string name, Sprite sprite, float cooldown, float boostDuration, PlayerController player) : base(name, sprite, cooldown, boostDuration, player)
    {
       projectiles =  GameObject.FindWithTag("RockPool").GetComponent<ProjectilePool>();
    }

    override
    public void activateGadget()
    {
        if (cooldownTimer == 0)
        {
            player.isAiming = true;
        }
    }

    override
    public void deactivateGadget()
    {
        cooldownTimer = cooldown;
    }


}
