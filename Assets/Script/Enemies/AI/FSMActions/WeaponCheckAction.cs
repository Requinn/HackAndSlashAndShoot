using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject;
using UnityEngine;

public class WeaponCheckAction : FsmStateAction {
    [RequiredField]
    [CheckForComponent(typeof(FoVDetection))]
    public FsmBool weaponReady;
    public FsmOwnerDefault gameobject;

    private Weapon weapon;
    // Use this for initialization
    public override void Awake () {
		weapon = Fsm.GetOwnerDefaultTarget(gameobject).GetComponent<MeleeRusher>().weapon;

    }
    public override void Reset() {
        gameobject = null;
        weaponReady = null;

    }
    public override void OnEnter() {
        CheckWeapon();
    }

    public override void OnUpdate() {
        CheckWeapon();
    }
    public void CheckWeapon(){
        if (weapon != null){
            weaponReady = weapon._canAttack;
        }
    }
}
