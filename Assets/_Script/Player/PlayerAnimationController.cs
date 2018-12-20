using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using System;

/// <summary>
/// Handles the animations for the player
/// </summary>
public class PlayerAnimationController : MonoBehaviour{
    private Animator _animator;
    private float _moveAngle, _moveLookDiff;
    public float lookAngle;
    public Vector3 moveVector;
	// Use this for initialization
	void Awake (){
	    _animator = GetComponent<Animator>();
	}

    void Update(){
        //check if we are moving in any direction
        if (moveVector.z != 0 || moveVector.x != 0){
            //check rotation from the player
            if (lookAngle < 0){
                lookAngle += 360;
            }

            //translate movement to an angle
            _moveAngle = Mathf.Atan2(moveVector.x, moveVector.z) / Mathf.PI * 180;
            if (_moveAngle < 0){
                _moveAngle += 360;
            }

            //get the difference in angle to get the direction of relative travel from look to movement (and clamp that shit)
            _moveLookDiff = _moveAngle - lookAngle;
            if (_moveLookDiff > 180){
                _moveLookDiff -= 360;
            }
            if (_moveLookDiff < -180){
                _moveLookDiff += 360;
            }
            _animator.SetFloat("LookDiff", _moveLookDiff);
        }
        else
        {
            _animator.SetFloat("LookDiff", 0f);
        }

    }

    //play a custom animation
    public void PlayAnim(string animName){
        _animator.Play(animName);
    }

    /// <summary>
    /// Play an animation based on the weapon's id
    /// </summary>
    /// <param name="weaponLayerID"></param>
    public void PlayIdle(int weaponLayerID) {
        _animator.Play("Idle", weaponLayerID);
    }

    public void PlayAttack(int weaponLayerID) {
        _animator.Play("Attack", weaponLayerID);
    }

    public void SetActiveLayer(int weaponLayerID) {
        Debug.Log(weaponLayerID + 1);
        int layerCount = _animator.layerCount;
        for (int i = 1; i < layerCount; i++) {
            if (i == weaponLayerID + 1) {
                _animator.SetLayerWeight(i, 1f);
            }
            else {
                _animator.SetLayerWeight(i, 0f);
            }
        }
    }
    //DEPRECATED BELOW------------------
    //movement animations
    public void WalkForward(){
        _animator.SetFloat("Speed", 1.0f);
    }

    public void WalkBackward(){ //prob unsused
        _animator.SetFloat("Speed", -1.0f);
    }

    public void WalkSide(){
        _animator.SetFloat("Side", 1f);
    }

    public void Idle(){
        _animator.SetFloat("Speed", 0f);
    }


    //Attacking animations
    public void GunShot() {
        _animator.Play("GunShot", 1);
        //_animator.SetTrigger("GunAttack");
    }

    public void SwordSwing(){
        _animator.SetTrigger("SwordAttack");
    }
}
