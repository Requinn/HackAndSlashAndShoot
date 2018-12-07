using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using MEC;
using System;

public class ImmobilisingMine : BreakableObject {
    [SerializeField]
    private float _catchDuration = 5f;
    [SerializeField]
    private float _floorDuration = 10f; //how long does this last on the ground
    [SerializeField]
    private float _trapActivationRadius = 0.75f; //how large is the trigger area
    [SerializeField]
    private GameObject _trapVisual;
    [SerializeField]
    private CapsuleCollider _breakHitBox; //this hitbox allows the player to hit the trap to break free

    private PlayerController _playerObj;
    private CoroutineHandle _selfDisableHandle; //store the coroutine's handle for disbaling ourself while we are on the floor

    private bool _activated = false;
    private bool _isBroken = false;
    private Collider[] _overlapHit; //used to check if we hit something, then extra controller from

    public void Update() {
        //do an overlap sphere, checking only for the player layer and if we get something, the array should only hold one thing
        if(!_activated) {
            _overlapHit = Physics.OverlapSphere(transform.position, _trapActivationRadius, 1 << LayerMask.NameToLayer("Player"));
            if (_overlapHit.Length > 0) {
                _playerObj = _overlapHit[0].GetComponent<PlayerController>();
                Timing.KillCoroutines(_selfDisableHandle); //stop running our self destroy
                ActivateTrap();
            }
        }

        //if we were activated, check if we aren't broken, if we aren't tick down to 0
        if(_activated && !_isBroken) {
            _catchDuration -= Time.deltaTime;
            if(_catchDuration <= 0f) {
                Break();
            }
        }
    }

    //when this is activated, count down until this trap auto breaks
    private void OnEnable() {
        _selfDisableHandle = Timing.RunCoroutine(DisableSelf());
    }

    /// <summary>
    /// Destroys this trap after some seconds of non interaction
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> DisableSelf() {
        yield return Timing.WaitForSeconds(_floorDuration);
        Break();
    }

    /// <summary>
    /// Hold the player in place
    /// </summary>
    private void ActivateTrap() {
        if (_playerObj) {
            _activated = true;
            Timing.RunCoroutine(ShiftPlayerToCenter());
            _trapVisual.SetActive(true);
            _playerObj.CanMove = false;
            _playerObj.GetComponent<ImpactReceiver>().enabled = false;
        }
    }

    /// <summary>
    /// Player triggered the trap, so slide them over to the center of the object
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> ShiftPlayerToCenter() {
        while(!AreFloatsNearlyEqual(_playerObj.transform.position.x, transform.position.x, 0.01f) && !AreFloatsNearlyEqual(_playerObj.transform.position.z, transform.position.z, 0.01f)) {
            _playerObj.transform.position = new Vector3(Mathf.Lerp(_playerObj.transform.position.x, transform.position.x, Time.deltaTime * 5f), _playerObj.transform.position.y, Mathf.Lerp(_playerObj.transform.position.z, transform.position.z, Time.deltaTime * 5f));
            yield return 0f;
        }
        //once the player is locked in, allow them to break out, this is after we move so we can avoid a tricky null error on breaking the thing while we're on the move
        //unless the trap is huge, should not be a problem
        _breakHitBox.enabled = true;
    }

    /// <summary>
    /// only register hits if the hitbox to take damage on this trap is enabled
    /// </summary>
    public override void Hit() {
        if (_breakHitBox.enabled) {
            base.Hit();
        }
    }
    /// <summary>
    /// broke the trap, we can move the player again
    /// </summary>
    public override void Break() {
        _isBroken = true;
        if (_playerObj) {
            _playerObj.CanMove = true;
            _playerObj.GetComponent<ImpactReceiver>().enabled = true;
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private bool AreFloatsNearlyEqual(float a, float b, float tolerance) {
        return Math.Abs(a - b) <= tolerance;
    }
}
