using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour {
    Animator animator;
    public int value;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }
	
	public int Interact()
    {
        animator.SetBool("Open", true);
        gameObject.layer = 14;

        return value;
    }
}
