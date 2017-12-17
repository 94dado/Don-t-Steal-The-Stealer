using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour {

    int nextProjectile = 0;
    List<ProjectileBehaviour> projectiles;

	// Use this for initialization
	void Start () {
        projectiles = new List<ProjectileBehaviour>();
        for (int i = 0; i < transform.childCount; i++)
            projectiles.Add(this.gameObject.transform.GetChild(i).GetComponent<ProjectileBehaviour>());
	}
	
	public ProjectileBehaviour getNextProjectile()
    {
        nextProjectile++;
        if (nextProjectile == transform.childCount + 1)
            nextProjectile = 1;
        return projectiles[nextProjectile - 1];
    }
}
