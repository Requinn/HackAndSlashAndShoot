using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using MEC;
using System;

public class ImmobilisingMine : BreakableObject {
    [SerializeField]
    private float _duration = 5f;
    [SerializeField]
    private GameObject _trapVisual;
    [SerializeField]
    private CapsuleCollider _triggerHitBox; //the hitbox to trigger this trap, maybe I could do something like overlap sphere or w/e
    [SerializeField]
    private CapsuleCollider _breakHitBox; //this hitbox allows the player to hit the trap to break free
    private PlayerController _playerObj;

    private void OnTriggerEnter(Collider c) {
        if(c.tag == "Player") {
            _triggerHitBox.enabled = false;
            //player stepped in, do stuff
            _playerObj = c.gameObject.GetComponent<PlayerController>();
            ActivateTrap();
            Timing.RunCoroutine(Disable());
        }
    }

    /// <summary>
    /// Disable ourself in some seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> Disable() {
        yield return Timing.WaitForSeconds(_duration);
        if (_playerObj) {
            _playerObj.CanMove = true;
        }
        if (!gameObject) {
            yield break;
        }
        gameObject.SetActive(false);
        yield return 0f;
        Destroy(gameObject);
    }

    /// <summary>
    /// Hold the player in place
    /// </summary>
    private void ActivateTrap() {
        if (_playerObj) {
            Timing.RunCoroutine(ShiftPlayerToCenter());
            _trapVisual.SetActive(true);
            _playerObj.CanMove = false;
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
    /// broke the trap, we can move the player again
    /// </summary>
    public override void Break() {
        if (_playerObj) {
            _playerObj.CanMove = true;
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private bool AreFloatsNearlyEqual(float a, float b, float tolerance) {
        return Math.Abs(a - b) <= tolerance;
    }
}
