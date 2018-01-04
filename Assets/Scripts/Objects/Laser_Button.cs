using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Button : InteractableObject
{

    GameObject[] laser;

    void Start()
    {
        laser = GameObject.FindGameObjectsWithTag("Laser");
    }

    override
    public int Interact()
    {
        foreach (GameObject l in laser)
        {
            l.GetComponent<SpriteRenderer>().enabled = true;
        }
        gameObject.layer = LayerMask.NameToLayer("Interacted");

        return 0;
    }
}
