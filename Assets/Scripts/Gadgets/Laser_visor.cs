using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_visor : Gadgets {

    GameObject[] laser;

    public Laser_visor(string name, Sprite sprite, float cooldown, float boostDuration, PlayerController player) : base(name, sprite, cooldown, boostDuration, player)
    {
        laser = GameObject.FindGameObjectsWithTag("Laser");
    }

    override
     public void activateGadget()
    {
        if (cooldownTimer == 0)
        {
            foreach (GameObject l in laser)
                l.GetComponent<SpriteRenderer>().enabled = true;
            cooldownTimer = cooldown;
        }
    }
}
