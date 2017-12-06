using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions {
    Up, Down, Left, Right
};

public class Torchlight : MonoBehaviour{

    public Transform up, down, left, right;

    private Dictionary<Directions, Transform> dict = new Dictionary<Directions, Transform>();
    private FSMEnemy AI;

    void Start() {
        dict.Add(Directions.Up, up);
        dict.Add(Directions.Down, down);
        dict.Add(Directions.Left, left);
        dict.Add(Directions.Right, right);
        AI = GetComponent<FSMEnemy>();
    }

    void Update() {
        //todo
    }
}
