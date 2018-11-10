using MEC;
using JLProject;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Bug: if you manage to line up the charge so that they end in a corner, they might get stuck?
public class TankyRanged : AIEntity {
    [SerializeField]
    private float _followingDistance = 10f;
    [SerializeField]
    private float _timeToRotate = 1.2f;
    [SerializeField]
    private float _maxChargeDistance = 15f;
    [SerializeField]
    private float _chargeCooldown = 10f;
    [SerializeField]
    private GameObject _damageBox; //this is enabled during the charge to do damage

    private float _currentCooldown = 0.0f;
    private bool _struck = false;
    private bool _isChargeInterrupted = false;
    //Future note remove this from AI refactor, only here bcause playmaker says so
    public override void ProjectileResponse() {
        throw new NotImplementedException();
    }

    protected override void Attack() {
        weapon.Fire();
    }

    // Use this for initialization
    new void Start () {
        base.Start();
        Timing.RunCoroutine(AICycle());
        TookDamage += TankyRanged_TookDamage;
    }

    //If we took damage, say we did and act on the next frame
    private void TankyRanged_TookDamage(Damage.DamageEventArgs args) {
        if (_currentCooldown >= _chargeCooldown) {
            _struck = true;
        }
    }

    //timer for the charge
    public void Update() {
        if(_currentCooldown <= _chargeCooldown) {
            _currentCooldown += Time.deltaTime;
        }
    }

    private CoroutineHandle ChargeHandle;
    private IEnumerator<float> AICycle() {
        //while alive
        while (!IsDead) {
            //if we weren't hit by an attack
            if (!_struck) {
                //just cycle move and attack
                Movement();
                if (weapon._canAttack) {
                    Attack();
                }
            //if we did get hit
            }else {
                //do the charge and pause this cycle until we're done with that
                ChargeHandle = Timing.RunCoroutine(ChargeTowardsPlayer());
                yield return Timing.WaitUntilDone(ChargeHandle);
            }
            yield return 0f;
        }
    }

    private RaycastHit _hit;
    /// <summary>
    /// Charge forward in the direction of the player, or until we hit a wall
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> ChargeTowardsPlayer() {
        Vector3 chargeDestination = transform.forward * _maxChargeDistance + transform.position; //get the offset of us towards the player in the distance, which we should be looking at
        chargeDestination.y = target.transform.position.y; //flatten the Y to be the same

        transform.LookAt(transform.forward);

        yield return Timing.WaitForSeconds(0.5f); //wait a little, then charge forward

        //check if theres a wall or something in the way after the pause, gotta look before you cross the street
        Physics.Raycast(transform.position, transform.forward, out _hit, _maxChargeDistance);
        //if we hit a wall move to where we hit that instead
        if (_hit.collider != null && _hit.collider.tag == "Environment") {
            Debug.Log(_hit.collider.name);
            chargeDestination.x = _hit.point.x;
            chargeDestination.z = _hit.point.z;
        }
        //activate our damaging hitbox
        _damageBox.SetActive(true);

        //Charge towards our destination, approximately within half a unit. a sloppy distance, but this charge was finicky in finer comparisons
        while (!AreFloatsNearlyEqual(transform.position.x, chargeDestination.x, 0.5f) && !AreFloatsNearlyEqual(transform.position.z, chargeDestination.z, 0.5f)){
            //_chargingTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, chargeDestination, Time.deltaTime * 15f);
            yield return 0f;
        }

        yield return Timing.WaitForSeconds(0.05f);
        //deactivate our damaging hitbox
        _damageBox.SetActive(false);

        _struck = false;
        _currentCooldown = 0.0f;
        yield return 0f;
    }

    /// <summary>
    /// Compares float a and b within a given tolerance.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="tolerance"></param>
    /// <returns></returns>
    private bool AreFloatsNearlyEqual(float a, float b, float tolerance) {
        return Math.Abs(a - b) <= tolerance;
    }

    protected override void Movement() {
        //find direction of movement that sets them to be a set distance away from the player
        //if we can see the player, just move to a point in direction * distance near them
        //if we can't, move there, until we can
        //from the player, get direction to us multiplied by distance, move to it

        //if we can't see the player, but we're in range, just head towards them
        if (!_vision.CanSeeTarget(target.transform)) {
            _NMAgent.SetDestination(target.transform.position);
        }
        //once we can see the player, 
        else {
            Rotate();
            //if we are within the following distance, don't really move
            float distanceSQ = (transform.position - target.transform.position).sqrMagnitude;
            if (distanceSQ > _followingDistance * _followingDistance) {
                Vector3 distanceOffsetToTarget = ((transform.position - target.transform.position).normalized * _followingDistance) + target.transform.position;
                _NMAgent.SetDestination(distanceOffsetToTarget);
            }
            else {
                _NMAgent.SetDestination(transform.position);
            }
            
        }
    }

    private Vector3 _pos, _dir;
    private Quaternion _lookrotation;
    /// <summary>
    /// Rotate to face the player at a speed
    /// </summary>
    protected void Rotate() {
        _pos = transform.position;
        _dir = (target.transform.position - _pos).normalized;       //direction to look at
        _lookrotation = Quaternion.LookRotation(_dir);              //generate a quaternion using the direction
        transform.DORotate(_lookrotation.eulerAngles, _timeToRotate);    //rotate towards it with a speed
    }


}
