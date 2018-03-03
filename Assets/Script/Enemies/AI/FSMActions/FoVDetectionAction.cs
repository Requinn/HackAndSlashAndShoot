using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using JLProject;
using UnityEngine;

public class FoVDetectionAction : FsmStateAction{
    [RequiredField] [CheckForComponent(typeof(FoVDetection))] public FsmBool canSee;
    [RequiredField] [CheckForComponent(typeof(FoVDetection))] public FsmBool canAttack;
    public FsmOwnerDefault gameobject;
    private FoVDetection FoVScript;
    private Transform _player;

    public override void Awake(){
        FoVScript = Fsm.GetOwnerDefaultTarget(gameobject).GetComponent<FoVDetection>();
        _player = GameObject.FindObjectOfType<PlayerController>().transform;
    }

    public override void Reset(){
        gameobject = null;
        canAttack = null;
        canSee = null;

    }

    public override void OnEnter(){
        CheckSight();
    }

    public override void OnUpdate(){
        CheckSight();
    }

    public void CheckSight(){
        canAttack.Value = FoVScript.inAttackCone;
        canSee.Value = FoVScript.CanSeeTarget(_player);
    }
}
