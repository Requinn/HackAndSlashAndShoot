using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIEntity : Entity{
    public Weapon weapon;
    protected NavMeshAgent _NMAgent;
    private float _evasionSpeed = 65f;
    private float _evasionDuration = 0.3f;
    private float _evadeDistance = 12.5f;
    private float _evadeCooldown = 1.0f;
    public bool _canEvade = true;
    /// <summary>
    /// this is for fsm experiments
    /// </summary>
    public void Move() {
        Movement();
    }

    //pushes the NavMeshAgent into a direction
    public virtual IEnumerator<float> Evade(Vector3 direction){
        _canEvade = false;
        _NMAgent.isStopped = true;
        _NMAgent.velocity = direction * _evadeDistance;
        _NMAgent.speed = _evasionSpeed;
        _NMAgent.angularSpeed = 0; //don't rotate around
        _NMAgent.acceleration = 25f; //speed up fast

        yield return Timing.WaitForSeconds(_evasionDuration);
        
        _NMAgent.isStopped = false;

        yield return Timing.WaitForSeconds(_evadeCooldown);
        _canEvade = true;


    }

}
