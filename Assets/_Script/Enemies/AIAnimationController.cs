using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationController : MonoBehaviour {
    private Animator _animator;
    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //play a custom animation
    public void PlayAnim(string animName) {
        _animator.Play(animName);
    }

    //DEPRECATED BELOW------------------
    //movement animations
    public void WalkForward() {
        _animator.SetFloat("Speed", 1.0f);
    }

    public void WalkBackward() { //prob unsused
        _animator.SetFloat("Speed", -1.0f);
    }

    public void WalkSide() {
        _animator.SetFloat("Side", 1f);
    }

    public void Idle() {
        _animator.SetFloat("Speed", 0f);
    }
}
