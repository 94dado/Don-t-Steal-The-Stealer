using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : InteractableObject {

    public int keyID;

    override
    public int Interact()
    {
        this.gameObject.SetActive(false);
        return value;
    }

    public int getKeyID()
    {
        return this.keyID;
    }

}
