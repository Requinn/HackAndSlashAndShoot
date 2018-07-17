using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HutongGames.PlayMaker.Actions;
using JLProject;
using UnityEngine;
using MEC;
using ProBuilder2.Common;

public class SweepingLaser : MonoBehaviour{
    //only need to use single floats as we rotate only on the Y plane
    public float initAngle = 60f; //what angle do we start at
    public float sweepAngle = 180f; //how much of an angle do we move
    public float timeToSweep = 7f; //how long it takes to finish the sweep
    public float damageTickRate = 3f; //how often we can deal damage
    public float reach = 30f;
    private bool _isDoneSweeping = false;
    private float _curSweepTime = 0f;
    private float _timeSinceLastDamageTick = 0f; //control how often something is hit by the beam, fixed instant melting

    private LineRenderer _lineRender;
    private Vector3 _origin;
    private Ray _ray;

    private RaycastHit _hit;
    private RaycastHit _playerHit;
    private Damage.DamageEventArgs args;

    private Sequence _rotationSequence;
    void Start(){
        args = new Damage.DamageEventArgs(50f, this.transform.position, Damage.DamageType.Neutral, Damage.Faction.Enemy);
        _lineRender = GetComponent<LineRenderer>();
        _origin = transform.position;
        _lineRender.positionCount = 1;
        _lineRender.SetPosition(0, _origin);
    }

    void Update() {
        if (_timeSinceLastDamageTick < damageTickRate){
            _timeSinceLastDamageTick += Time.deltaTime;
        }
        
    }
    //Consider using an animation here to do a rotation that isn't in a wrong direction
    /// <summary>
    /// Start a laser sweep with these parameters
    /// </summary>
    /// <param name="startAngle"></param>
    /// <param name="sweepAmount"></param>
    /// <param name="sweepTime"></param>
    public void StartSweep(float startAngle = 0f, float sweepAmount = 90f, float sweepTime = 7f){
        DOTween.Kill(this);
        _isDoneSweeping = false;
        initAngle = startAngle;
        sweepAngle = sweepAmount;
        timeToSweep = sweepTime;
        transform.rotation = Quaternion.Euler(0, initAngle, 0);
        _curSweepTime = 0f;
        Timing.RunCoroutine(SweepRoutine());
        _rotationSequence.Append(transform.DORotate(new Vector3(0, sweepAngle + initAngle, 0), timeToSweep).SetEase(Ease.Linear));//do the tween here so it doesn't break
    }

    ///handles when to show and disable the laser
    IEnumerator<float> SweepRoutine(){
        while (!_isDoneSweeping && this) { //only do the sweep when we're alive
            CalculateHit();
            _curSweepTime += Time.deltaTime;
            if (_curSweepTime > timeToSweep) {
                DisableLaser();
                _isDoneSweeping = true;
            }
            yield return 0f;
        }
        
        yield return 0f;
    }

    /*
    //if player collides with the laser hitbox, and is hittable, then do damage.
    //using on trigger stay so the player can take damage while sitting in the laser
    void OnTriggerStay(Collider c){
        PlayerController p = c.GetComponent<PlayerController>();
        if (p){
            //check a straight ray from source to player, as using the forward will create a form of race condition where by the time you check
            //the player's position, the forwards is somewhere else
            if (Physics.Raycast(new Ray(transform.position, (p.transform.position - transform.position).normalized), out _playerHit, reach)){
                if (_timeSinceLastDamageTick >= damageTickRate && _playerHit.transform.CompareTag("Player")){
                    p.TakeDamage(this, ref args);
                    _timeSinceLastDamageTick = 0;
                }
            }
        }
    }*/

    /// <summary>
    /// draw a laserfrom self to end of raycast
    /// </summary>
    void CalculateHit(){
        _lineRender.SetPosition(0, _origin);

        _ray.origin = transform.position;
        _ray.direction = transform.forward;

        //inconsistent hit detection on the player
        //inconsistent wall detection now?
        //used to render the laser's end point using an array to render through enemies/player
        if (Physics.Raycast(_ray, out _hit, reach)){
            Transform hitComponent = _hit.transform;
            _lineRender.positionCount = 2;
            _lineRender.SetPosition(1, _hit.point);
            //if (hitComponent.CompareTag("Environment")){ //a redundancy, maybe used to make it so the laser will pass through the player
            //    _lineRender.SetPosition(1, _hit.point);
            //}
            if (_timeSinceLastDamageTick >= damageTickRate && hitComponent.transform.CompareTag("Player")){
                hitComponent.GetComponent<PlayerController>().TakeDamage(this, ref args);
                _timeSinceLastDamageTick = 0;
            }
        }
        else{
            _lineRender.positionCount = 2;
            _lineRender.SetPosition(1, _origin + _ray.direction * reach);
        }
        /**
        foreach (var v in _hits){
            Transform hitComponent = v.transform;

            if (hitComponent.CompareTag("Environment")){
                _lineRender.SetPosition(1, v.point);
                if (hitComponent.name == "Barrier"){
                    Debug.Log("Blocked");
                }
                break;
            }
        }*/
    }

    void DisableLaser(){
        _lineRender.positionCount = 1;
        _lineRender.SetPosition(0, _origin);
    }
}
