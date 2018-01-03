using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Player"))
        {
            gameManager.gameOver = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
