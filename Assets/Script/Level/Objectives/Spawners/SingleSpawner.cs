using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSpawner : MonoBehaviour{
    public GameObject SpawnedObj;
    void OnEnable(){
        SpawnObj();
    }

    void SpawnObj(){
        SpawnedObj = Instantiate(SpawnedObj, transform.position, Quaternion.identity);
    }
}
