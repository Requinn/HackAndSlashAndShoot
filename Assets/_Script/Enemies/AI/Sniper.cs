using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

/// <summary>
/// Sniper Enemy. Long Range, high veloict and damage shots.
/// Will cloak when player enter melee range.
/// Maybe add some kind of defensive buff whil cloaked.
/// </summary>
public class Sniper : AIEntity{
    [Header("Sniper Stats")]
    private LineRenderer _line;
    public float aimTime = 1.5f;
    public float minDistance = 5f;
    private bool _isDoneAiming = false;
    private CoroutineHandle _aimHandle;
    private Cloak cloak;
    private float _curAimTime = 0f;

	// Use this for initialization
    new void Start (){
	    base.Start();
	    cloak = GetComponent<Cloak>();
	    _line = GetComponent<LineRenderer>();
	    _line.positionCount = 2;
	}

    public override void ProjectileResponse(){
        //nothing for now
    }

    // Update is called once per frame
	void Update () {
	    if (_vision.CanSeeTarget(target.transform) && !cloak.isCloaked){
	        transform.LookAt(target.transform);
            if (weapon._canAttack){
	            _line.enabled = true;
	            _line.SetPosition(0, transform.position);
	            _line.SetPosition(1, target.transform.position);

	            if (_curAimTime < aimTime){
	                _curAimTime += Time.deltaTime;
	            }

	            //if (!_isDoneAiming){
	              //  _aimHandle = Timing.RunCoroutine(StartAiming());
	            //}
                
                if (_curAimTime >= aimTime){
                    weapon.Fire();
                    _line.enabled = false;
                    _curAimTime = 0f;
                }
	        }
        }
	    else{
	        _line.enabled = false;
	        Timing.KillCoroutines(_aimHandle);
	    }

	    if (!weapon._canAttack || cloak.isCloaked){
	        if ((Vector3.SqrMagnitude(target.transform.position - transform.position) <
	             minDistance * minDistance)){
	            if (!cloak.isCloaked){ //cloak if we aren't cloaked
	                cloak.StartCloak();
	            }
                Movement();
	        }
	    }
		//if can attack
            //render line to player for 1.5s(?)
                //fire at player's position

        //if player enteres min range
            //become invisible and run to a location that is X distance from the player, but still on the navmesh
	}

    private IEnumerator<float> StartAiming(){
        _isDoneAiming = false;

        yield return Timing.WaitForSeconds(aimTime);
        _isDoneAiming = true;
    }

    protected override void Movement(){
        Vector3 targetPosition = target.transform.position;
        targetPosition.y = transform.position.y;
        Vector3 pointAwayFromTarget = (transform.position - targetPosition).normalized;
        transform.forward = pointAwayFromTarget;
        _NMAgent.Move(pointAwayFromTarget * MovementSpeed * Time.deltaTime);
    }

    protected override void Attack(){
        throw new System.NotImplementedException();
    }
}
