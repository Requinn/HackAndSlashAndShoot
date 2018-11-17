using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// a kill-type objective for testing purposes
/// </summary>
public class AmbushObjective : LevelObjective, IKillObjective{
    public bool PreSpawn;
    public List<SingleSpawner> EnemyList = new List<SingleSpawner>();
    [SerializeField]
    private NavMeshSurface _surface;
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
        //Update a navmesh incase we spawn on some moving object
        if (_surface) {
            _surface.BuildNavMesh();
        }
        foreach (var enemy in EnemyList){
            enemy.gameObject.SetActive(true);
            enemy.SpawnedObj.GetComponent<Entity>().OnDeath += MarkDead;
            enemy.SpawnedObj.GetComponent<Entity>().OnRevive += MarkLive;
        }
        _spawned = true;
        if (ObjectToLock){
            ObjectToLock.Close();
        }
    }

    public override void Initiate(){
        base.Initiate();
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
            foreach (var enemy in EnemyList) {
                enemy.gameObject.SetActive(false);
            }
            OpenDoors();
            OnCompleteObjective();
            isObjectiveComplete = true;
        }
    }

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == "Player") {
            Initiate();
            GetComponent<Collider>().enabled = false;
        }
    }
}
