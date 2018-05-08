using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;
/// <summary>
/// a kill-type objective for testing purposes
/// </summary>
public class AmbushObjective : LevelObjective, IKillObjective{
    public bool PreSpawn;
    public List<SingleSpawner> EnemyList = new List<SingleSpawner>();
    //public Unlockable ObjToUnlock;
    [SerializeField] private int _deathCount = 0;
    private bool _spawned = false;
    public void Start(){
        if (PreSpawn && !_spawned){
            Spawn();
        }
    }

    /// <summary>
    /// spawn the enemies
    /// </summary>
    public void Spawn(){
        foreach (var enemy in EnemyList){
            enemy.gameObject.SetActive(true);
            enemy.SpawnedObj.GetComponent<Entity>().OnDeath += MarkDead;
            enemy.SpawnedObj.GetComponent<Entity>().OnRevive += MarkLive;
        }
        _spawned = true;
        ObjectToLock.Close();
    }

    public override void Initiate(){
        if (!PreSpawn && !_spawned){
            Spawn();
        }
    }
    /// <summary>
    /// called when something dies
    /// </summary>
    private void MarkDead(){
        _deathCount++;
        CheckCompletion();
    }
    /// <summary>
    /// called when something is revived
    /// </summary>
    private void MarkLive(){
        _deathCount--;
    }

    public void CheckCompletion(){
        if (_deathCount == EnemyList.Count){
            if (ObjectToUnlock){
                ObjectToUnlock.Locked = false;
                ObjectToUnlock.Open();
            }
            OnCompleteObjective();
        }
    }

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == "Player") {
            Initiate();
            GetComponent<Collider>().enabled = false;
        }
    }
}
