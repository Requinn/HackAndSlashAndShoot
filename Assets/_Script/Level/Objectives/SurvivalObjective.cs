using System;
using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

public class SurvivalObjective : LevelObjective{
    public List<SpawnWave> waves = new List<SpawnWave>();
    public int currentWave = 0;
    public bool oneWay = true;
    private bool completed = false;

    public override void Initiate(){
        if (ObjectToLock){
            ObjectToLock.Locked = true;
            ObjectToLock.Close();
        }
        waves[currentWave].Spawn();
        foreach (var w in waves){
            w.OnCompleteWave += StartNextWave;
        }
    }

    private void StartNextWave(){
        if (currentWave < waves.Count - 1){
            currentWave++;
            waves[currentWave].Spawn();
        }
        else{
            OpenDoors();
            completed = true;
            OnCompleteObjective();
        }
    }

    private void OpenDoors(){
        if (ObjectToUnlock){
            ObjectToUnlock.Locked = false;
            ObjectToUnlock.Open();
        }
        if (!oneWay && ObjectToLock){
            ObjectToLock.Locked = false;
            ObjectToLock.Open();
        }
    }

    void OnTriggerEnter(Collider c){
        if (c.gameObject.tag == "Player" && !completed){
            Initiate();
            GetComponent<Collider>().enabled = false;
        }
    }
}
