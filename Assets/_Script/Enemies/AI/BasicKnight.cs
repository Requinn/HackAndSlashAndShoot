using System;
using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

public class BasicKnight : MeleeUnit{
    [SerializeField] private float _blockDuration = 1f;
    [SerializeField] private float _blockCooldown = 5f;
    public bool _canBlock = true;

    public float shieldedSpeed = 2.0f;
    // Update is called once per frame
    void Update(){
        ProjectileDetection();
    }

    protected void ProjectileDetection(){
        if (_canBlock) { 
            surroundingObjects = Physics.SphereCastAll(transform.position, 4f, Vector3.up);
            foreach (RaycastHit obj in surroundingObjects){
                if (obj.transform.gameObject.tag == "PlayerProjectile"){
                    thisFSM.SendEvent("ProjectileDetectedEvent");
                    break;
                }
            }
        }
    }

    //pushes the NavMeshAgent into a direction
    public override void ProjectileResponse(){
        Timing.RunCoroutine(StartBlock());

    }

    private IEnumerator<float> StartBlock(){
        _canBlock = false;
        shield.blocking = true;
        speed = shieldedSpeed;
        yield return Timing.WaitForSeconds(_blockDuration);
        shield.blocking = false;
        speed = movementSpeed;
        yield return Timing.WaitForSeconds(_blockCooldown);
        _canBlock = true;
    }
}
