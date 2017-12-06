using System.Collections.Generic;
using UnityEngine;

public enum Directions {
    Up, Down, Left, Right
};

public class Torchlight : MonoBehaviour{

    //the torchlights (one for every direction)
    public Transform up, down, left, right;

    //dictionary to store the directions
    private Dictionary<Directions, Transform> dict = new Dictionary<Directions, Transform>();
    //AI script
    private FSMEnemy AI;
    //direction of the last frame
    private Directions oldDirection;

    void Start() {
        //save the directions in the dictionary
        dict.Add(Directions.Up, up);
        dict.Add(Directions.Down, down);
        dict.Add(Directions.Left, left);
        dict.Add(Directions.Right, right);
        //get the AI component
        AI = GetComponent<FSMEnemy>();
        //setup the oldDirection with the direction of the prefab
        oldDirection = Directions.Down;
    }

    void Update() {
        //get from the current direction of the AI
        Directions currentDirection = AI.GetCurrentDirection();
        if(currentDirection != oldDirection) {
            //changed direction. Update torchlights
            dict[oldDirection].gameObject.SetActive(false);
            dict[currentDirection].gameObject.SetActive(true);
            oldDirection = currentDirection;
        }
    }
}
