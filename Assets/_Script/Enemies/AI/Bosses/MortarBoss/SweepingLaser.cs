using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JLProject;
using UnityEngine;
using MEC;

public class SweepingLaser : MonoBehaviour{
    //only need to use single floats as we rotate only on the Y plane
    public float initAngle = 60f; //what angle do we start at
    public float sweepAngle = 180f; //how much of an angle do we move
    public float timeToSweep = 7f; //how long it takes to finish the sweep
    public float damageTickRate = 3f; //how often we can deal damage
    public bool canReflect = false; //does the laser bounce?
    public float reach = 30f;
    private bool _isDoneSweeping = false;
    private float _curSweepTime = 0f;
    private float _timeSinceLastDamageTick = 0f; //control how often something is hit by the beam, fixed instant melting

    private LineRenderer _lineRender;
    private Vector3 _origin;
    private Ray _ray;
    [SerializeField]
    private RaycastHit[] _hits;

    private Damage.DamageEventArgs args;

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
   
        if (!_isDoneSweeping){
            _curSweepTime += Time.deltaTime;
            transform.DORotate(new Vector3(0, sweepAngle + initAngle, 0), timeToSweep).SetEase(Ease.Linear);
            //CalculateHit();
            if (_curSweepTime > timeToSweep){
                _isDoneSweeping = true;
            }
        }
        CalculateHit();
    }

    public void StartSweep(){
        _isDoneSweeping = false;
        _curSweepTime = 0f;
    }

    /// <summary>
    /// draw a laserfrom self to end of raycast
    /// </summary>
    void CalculateHit(){
        _lineRender.positionCount = 2;
        
        _ray.origin = transform.position;
        _ray.direction = transform.forward;
        _lineRender.SetPosition(0, _origin);
        _lineRender.SetPosition(1, _origin + _ray.direction * 30f);

        _hits = Physics.RaycastAll(_ray, reach);

        //inconsistent hit detection on the player
        foreach (var v in _hits){
            Debug.Log(v.transform.name);
            Transform hitComponent = v.transform;
            if (hitComponent.CompareTag("Environment")){
                _lineRender.SetPosition(1, v.point);
                break;
            }
            if (_timeSinceLastDamageTick >= damageTickRate && hitComponent.CompareTag("Player")){
                
                hitComponent.GetComponent<PlayerController>().TakeDamage(this, ref args);
                _timeSinceLastDamageTick = 0;
                break;
            }
        }
    }
}
