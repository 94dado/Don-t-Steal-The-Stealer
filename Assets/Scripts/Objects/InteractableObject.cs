﻿using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected Animator animator;
    public int value;
    protected GameObject child;
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

    virtual
    public int Interact()
    {
        if (animator != null)
            animator.SetBool("interacted", true);
       

        if (child != null)
            child.GetComponent<SpriteRenderer>().sprite = stolenSprite;
        else if (stolenSprite != null)
        {
            //this code resizes the painting and the box collider (in case the object is a painting)
            int oldHeight = this.gameObject.GetComponent<SpriteRenderer>().sprite.texture.height;
            int oldWidth = this.gameObject.GetComponent<SpriteRenderer>().sprite.texture.width;
            float oldColliderScaleX = this.gameObject.GetComponent<BoxCollider2D>().size.x;
            float oldColliderScaleY = this.gameObject.GetComponent<BoxCollider2D>().size.y;

            float newWidth = (this.gameObject.transform.localScale.x * oldWidth) / stolenSprite.texture.width;
            float newHeight = (this.gameObject.transform.localScale.y * oldHeight) / stolenSprite.texture.height;
            float newColliderWidth = (oldColliderScaleX * stolenSprite.texture.width) / oldWidth;
            float newColliderHeight = (oldColliderScaleY * stolenSprite.texture.height) / oldHeight;

            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(newColliderWidth, newColliderHeight);

            this.gameObject.transform.localScale = new Vector3(newWidth ,newHeight,1F);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = stolenSprite;
        }

        gameObject.layer = LayerMask.NameToLayer("Interacted");

        return value;
    }

   

    public string getObjectType()
    {
        return this.objectType;
    }

 
}

