using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

public class DodgyRusher : MeleeUnit {
    private float _evasionSpeed = 65f;
    [SerializeField]
    private float _evasionDuration = 0.3f;
    private float _evadeDistance = 12.5f;
    [SerializeField]
    private float _evadeCooldown = 1.0f;
    public bool _canEvade = true;

    private Vector3 _evadeDir;
    // Update is called once per frame
    void Update() {
        ProjectileDetection();
    }

    protected void ProjectileDetection() {
        if (_canEvade){
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
        if (this){
            Timing.RunCoroutine(Evade());
        }
    }

    private IEnumerator<float> Evade(){
        if (_NMAgent){
            if (Random.Range(0, 2) == 1){
                _evadeDir = -transform.right;
            }
            else{
                _evadeDir = transform.right;
            }
            _canEvade = false;
            _NMAgent.isStopped = true;
            _NMAgent.velocity = _evadeDir * _evadeDistance;
            _NMAgent.speed = _evasionSpeed;
            _NMAgent.angularSpeed = 0; //don't rotate around
            _NMAgent.acceleration = 25f; //speed up fast

            yield return Timing.WaitForSeconds(_evasionDuration);

            if (this){
                _NMAgent.isStopped = false;
            }
            yield return Timing.WaitForSeconds(_evadeCooldown);
            _canEvade = true;
        }
    }
}
