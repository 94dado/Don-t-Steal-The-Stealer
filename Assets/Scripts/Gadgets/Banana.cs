using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Gadgets {



    public Banana(string name, Sprite sprite, float cooldown, float boostDuration, PlayerController player) : base(name, sprite, cooldown, boostDuration, player)
    {
        projectiles = GameObject.FindWithTag("BananaPool").GetComponent<ProjectilePool>();
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
