using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gadgets  {

    public string name;
    public Sprite sprite;
    public float cooldown;
    public float cooldownTimer;
    public float boostDuration;
    public float boostDurationTimer;
    [HideInInspector]
    public ProjectilePool projectiles;
    protected PlayerController player;

    

    public Gadgets(string name, Sprite sprite,float cooldown, float boostDuration, PlayerController player)
    {
        this.name = name;
        this.sprite = sprite;
        this.cooldown = cooldown;
        this.boostDuration = boostDuration;
        this.player = player;
        this.cooldownTimer = 0;
        this.boostDurationTimer = 0;
    }
	

	public void UpdateCooldown (float deltaTime) {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= deltaTime;
        }

        if (cooldownTimer < 0)
        {
            cooldownTimer = 0;
        }


        if (boostDurationTimer > 0)
            boostDurationTimer -= deltaTime;

        if (boostDurationTimer < 0)
        {
            boostDurationTimer = 0;
            deactivateGadget();
        }
    }

    virtual
    public void activateGadget()
    {

    }

    virtual
    public void deactivateGadget()
    {

    }
}
