using System;
using System.Linq;
using UnityEngine;
using JLProject;
using MEC;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles a zone that will spawn enemies and optionally shrink as they are killed inside
/// </summary>
public class SacrificeZone : LevelObjective {
    [SerializeField]
    private List<WeightedSpawn> _weightedSpawnList; //what enemy and how often can they spawn?
    [SerializeField]
    private Transform[] _spawnPositions; //where can we spawn?
    [SerializeField]
    private int _killThreshold; //how many kills total should we make?
    [SerializeField]
    private int _thresholdChanges; //after how many kills does the acceptance radius shrink?
    [SerializeField]
    private int _maxSpawnCount; // how many can we have spawned at any one time?
    [SerializeField]
    private float _minSpawnDelay, _maxSpawnDelay; // how much time in between spawns do we have
    [SerializeField]
    private float _acceptanceRadius = 4f, _radiusShrinkValue = .2f; //how big is the area we can accept a kill, how much of it shrinks per threshold

    private float _minWeight, _maxWeight;
    private SphereCollider _collider;
    private int _deathCount = 0;
    public Action OnCompleteSacrifice = delegate { };

    public void Start() {
        //get our collider to shrink later
        _collider = GetComponent<SphereCollider>();
        if(_collider == null) {
            _collider = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
            _collider.isTrigger = true;
        }
        _collider.radius = _acceptanceRadius;
        //sort by weights, lowest to highest
        _weightedSpawnList = _weightedSpawnList.OrderBy(i => i.spawnWeight).ToList();
        //acquire min max values
        _minWeight = _weightedSpawnList[0].spawnWeight;
        _maxWeight = _weightedSpawnList[_weightedSpawnList.Count - 1].spawnWeight;

        InitializeCircle();
    }

    /// <summary>
    /// Generate a random weight value, then spawn an entity based on that value
    /// </summary>
    public void InitializeCircle() {
        for(int i = 0; i < _maxSpawnCount; i++) {
            GenerateSpawn();
        }
    }

    /// <summary>
    /// Generate a weight, then spawn something from it
    /// </summary>
    private void GenerateSpawn() {
        if (!gameObject.activeInHierarchy) { return; }
        float weight = UnityEngine.Random.Range(_minWeight, _maxWeight);
        Timing.RunCoroutine(SpawnEntity(GetEntityByWeight(weight)));
    }

    /// <summary>
    /// Spawn in enemies with a random delay
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private IEnumerator<float> SpawnEntity(Entity e) {
        yield return Timing.WaitForSeconds(UnityEngine.Random.Range(_minSpawnDelay, _maxSpawnDelay));
        int randPoint = UnityEngine.Random.Range(0, _spawnPositions.Length);
        Entity spawn = Instantiate(e, _spawnPositions[randPoint].position, Quaternion.identity);
        spawn.OnDeath += GenerateSpawn; //when we die and this object is still active, spawn another
        yield return 0f;
    }

    /// <summary>
    /// WCheck for completion every time this is called by something dying near it
    /// </summary>
    private void HandleDead() {
        _deathCount++;
        //if the threshold is met, shrink area, then set new threshold to be that vlaue ahead of thecurrent value
        if(_deathCount >= _thresholdChanges) {
            _collider.radius = _collider.radius - _radiusShrinkValue; //shrinkt he radius
            _thresholdChanges += _deathCount; //set new threshold to be current deathcount + threshold value
        }
        //if the death count is greater than the total kills needed, shut off after sending the completion event
        if (_deathCount >= _killThreshold) {
            OnCompleteSacrifice();
            OnCompleteObjective();
            ObjectToUnlock.Open();
            gameObject.SetActive(false);
            return;
        }
    }

    /// <summary>
    /// Since entity list is sorted by weight, low to high, if the weight is lower than the currently looked at value, spawn it, if not move and do another check
    /// if that weight is less than the next enity, but still higher than the previous, return that
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    private Entity GetEntityByWeight(float weight) {
        foreach(var e in _weightedSpawnList) {
            if(weight < e.spawnWeight) {
                //randomly spawn an enemy from that weight tier
                return e.availableEntities[UnityEngine.Random.Range(0, e.availableEntities.Length - 1)];
            }
        }
        int randIndex = UnityEngine.Random.Range(0, _weightedSpawnList[_weightedSpawnList.Count - 1].availableEntities.Length - 1);
        return _weightedSpawnList[_weightedSpawnList.Count-1].availableEntities[randIndex]; //default return the most common enemy incase something goes wrong
    }

    //When an enemy enters our circle area, subscribe to their death
    private void OnTriggerEnter(Collider other) {
        Entity e = other.GetComponent<Entity>();
        if (e) {
            if (e.Faction == Damage.Faction.Enemy) {
                e.OnDeath += HandleDead;
            }
        }
    }

    //if enemy leaves, unsub to their death
    private void OnTriggerExit(Collider other) {
        Entity e = other.GetComponent<Entity>();
        if (e) {
            if (e.Faction == Damage.Faction.Enemy) {
                e.OnDeath -= HandleDead;
            }
        }
    }
}

/// <summary>
/// Container to hold entities that can spawn at certain weights
/// </summary>
[System.Serializable]
public class WeightedSpawn {
    public float spawnWeight;
    public Entity[] availableEntities;

    public WeightedSpawn(Entity[] entity, float weight) {
        availableEntities = entity;
        spawnWeight = weight;
    }
}
