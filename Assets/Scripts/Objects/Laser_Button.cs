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
            l.SetActive(false);
        }
        gameObject.layer = LayerMask.NameToLayer("Interacted");
        GetComponent<SpriteRenderer>().sprite = stolenSprite;
        return 0;
    }
}
