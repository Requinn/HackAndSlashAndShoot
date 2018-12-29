using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using JLProject.Weapons;
using JLProject;

/// <summary>
/// weapon for the first boss to produce a crossing laser than will track the player until halfway through through its casting time, after which it will lock to that position
/// </summary>
public class Boss1TrackingBeam : Weapon {
    [Header("TrackingBeam")]

    [SerializeField]
    private LaserBeam _horizontal, _vertical;
    private float _trackingDelay = 0.1f;
    private float _timeToLockIn = 1f;

    public override void Fire() {
        Timing.RunCoroutine(TrackingRoutine());
    }

    private IEnumerator<float> TrackingRoutine() {
        float elapsedTime = 0f;
        //fire both laser
        _horizontal.Fire();
        _vertical.Fire();
        //start tracking
        while(elapsedTime < _timeToLockIn) {
            elapsedTime += Time.deltaTime;
            //track the player's vertical movements for the horizontal laser
            _horizontal.transform.position = new Vector3(
                _horizontal.transform.position.x ,
                _horizontal.transform.position.y,
                GameController.Controller.PlayerReference.transform.position.z);
            //track the player's horizontal movements for the vertical laser
            _vertical.transform.position = new Vector3(
                GameController.Controller.PlayerReference.transform.position.x,
                _vertical.transform.position.y,
                _vertical.transform.position.z);
            yield return 0f;
        }
        yield return 0f;
    }

    private void Start() {
        //get laser casting time and sync both lasers to horizontal's value;
        _timeToLockIn = _vertical.CastTime = _horizontal.CastTime;
        _timeToLockIn = _timeToLockIn * 3f/5f; //cut cast time in half as that is when we stop tracking
    }
}
