using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    Animator animator;
    public int value;
    private GameObject child;
    public Sprite stolenSprite;
    public string objectType;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        if (this.gameObject.transform.childCount != 0)
        {
            child = this.gameObject.transform.GetChild(0).gameObject;
        }
    }

    public int Interact()
    {
        if (animator != null)
            animator.SetBool("interacted", true);
        if (child != null)
            child.GetComponent<SpriteRenderer>().sprite = stolenSprite;
        else if (stolenSprite != null )
            this.gameObject.GetComponent<SpriteRenderer>().sprite = stolenSprite;

        gameObject.layer = 14;

        return value;
    }

    public string getObjectType()
    {
        return this.objectType;
    }
}

