using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour{
    private Animator _animator;
	// Use this for initialization
	void Start (){
	    _animator = GetComponent<Animator>();
	}

    public void WalkForward(){
        _animator.SetFloat("Speed", 1.0f);
    }
}
