using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the animations for the player
/// </summary>
public class PlayerAnimationController : MonoBehaviour{
    private Animator _animator;
    private float _moveAngle, _moveLookDiff;
    public float lookAngle;
    public Vector3 moveVector;
	// Use this for initialization
	void Start (){
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
            _moveAngle = Mathf.Atan2(moveVector.z, moveVector.x) / Mathf.PI * 180;
            if (_moveAngle < 0){
                _moveAngle += 360;
            }

            //get the difference in angle to get the inbetween "state" of directional movement
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
        //anim.Play("gun shooting animation");
        _animator.SetTrigger("GunAttack");
    }

    public void SwordSwing(){
        _animator.SetTrigger("SwordAttack");
    }
}
