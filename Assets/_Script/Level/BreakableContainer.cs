using System;
using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

public class BreakableContainer : BreakableObject {
    [Range(1,9)]
    public int AmountToSpawn;
    public HealthPickup pickUptoSpawn;

    private Vector3[] _spawnPositions = {
        new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(-1,0,0),
        new Vector3(0,0,1), new Vector3(0,0,-1), new Vector3(1,0,1),
        new Vector3(-1,0,1), new Vector3(1,0,-1), new Vector3(-1,0,-1)
    };

    public override void Break(){
        for (int i = 0; i < AmountToSpawn; i++){
            Instantiate(pickUptoSpawn, transform.position + _spawnPositions[i] + new Vector3(0, 0.875f, 0), Quaternion.identity);
        }
        base.Break();
    }

}
