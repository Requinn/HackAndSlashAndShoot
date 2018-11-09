using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using System;
using MEC;

public class OrbiterAttackDrone : AIEntity {
    [Header("Orbiter Drone Properties")]
    [SerializeField]
    float _maximumPlayerOrbitDistance = 5f; //how far away fromt he player should we be orbiting them
    [SerializeField]
    float _orbitAdjustmentDelay = 1f; //how quickly should we adjust ourselves to the player's new position
    [SerializeField]
    float _orbitSpeed = 4f;
    [SerializeField]
    float _speed = 3f;

    private float _startChaseDistanceDiff = 2f; //how far away this enemy must be on top of the max orbit distance before they are considered to not be orbiting
    private bool _isOrbiting = false;
    public override void ProjectileResponse() {
        
        //do nothing
    }

    protected override void Attack() {
        weapon.Fire();
    }

    private float _currentDistToPlayerSQ = 0f;
    private float _time;
    protected override void Movement() {
        transform.LookAt(target.transform);
        _time += Time.deltaTime;
        //if not orbiting the player, find them
        //if in orbit range, begin orbiting
        _currentDistToPlayerSQ = Vector3.SqrMagnitude(transform.position - target.transform.position);
        float playerDistSQ = _maximumPlayerOrbitDistance * _maximumPlayerOrbitDistance;

        if (_currentDistToPlayerSQ > playerDistSQ + (_startChaseDistanceDiff * _startChaseDistanceDiff)) {
            _isOrbiting = false;
            _NMAgent.Move(transform.forward * MovementSpeed * Time.deltaTime);
        }
        else {
            transform.RotateAround(target.transform.position, Vector3.up, _orbitSpeed * Time.deltaTime);
            Vector3 adjustedPosition = ((transform.position - target.transform.position).normalized * _maximumPlayerOrbitDistance) + target.transform.position;
            transform.position = Vector3.Lerp(transform.position, adjustedPosition, Time.deltaTime * _orbitAdjustmentDelay);
            _NMAgent.updatePosition = true; //maybe this will fix the jumpiness
            _isOrbiting = true;
        }
        
    }

    // Use this for initialization
    void Start () {
        Timing.RunCoroutine(AICycle());
        MovementSpeed = _speed;
	}

    private IEnumerator<float> AICycle() {
        while(!IsDead && target != null) {
            //if not orbiting, fire weapon every 5 seconds
            if (_isOrbiting && weapon._canAttack) {
                Attack();
            }
            //move always, whether to or around the player
            Movement();
            yield return 0f;
        }
    }

    // Update is called once per frame
    void Update () {
		//if orbiting, attack player every 5 seconds
	}
}
