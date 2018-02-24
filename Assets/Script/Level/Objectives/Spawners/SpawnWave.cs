using System;
using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

public class SpawnWave : MonoBehaviour {
    public List<SingleSpawner> spawners = new List<SingleSpawner>();
    private int count = 0;
    public bool complete = false;

    protected Action onCompleteWave = delegate { };
    public Action OnCompleteWave {
        get { return onCompleteWave; }
        set { onCompleteWave = value; }
    }

    /// <summary>
    /// spawns a wave
    /// </summary>
    /// <returns></returns>
    public void Spawn(){
        foreach (var s in spawners){
            s.gameObject.SetActive(true);
            s.SpawnedObj.GetComponent<Entity>().OnDeath += HandleDeath;
            s.SpawnedObj.GetComponent<Entity>().OnRevive += HandleRes;
        }
    }

    private void HandleRes(){
        count--;
    }

    private void HandleDeath(){
        count++;
        CheckComplete();
    }


    void CheckComplete(){
        if (count == spawners.Count){
            OnCompleteWave();
        }
    }
}
