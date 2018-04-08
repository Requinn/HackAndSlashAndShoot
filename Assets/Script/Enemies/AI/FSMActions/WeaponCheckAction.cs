using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject;
using UnityEngine;

public class WeaponCheckAction : FsmStateAction {
    [RequiredField]
    [CheckForComponent(typeof(FoVDetection))]
    public FsmBool weaponReady;
    public FsmBool inCombo;
    public FsmOwnerDefault gameobject;

    private Weapon weapon;
    // Use this for initialization
    public override void Awake () {
        weapon = Fsm.GetOwnerDefaultTarget(gameobject).GetComponent<AIEntity>().weapon;
    }

    public override void OnEnter() {
        weapon = Fsm.GetOwnerDefaultTarget(gameobject).GetComponent<AIEntity>().weapon;
        CheckWeapon();
        Finish();
    }

    public override void OnUpdate() {
        CheckWeapon();
    }
    public void CheckWeapon(){
        if (weapon != null){
            weaponReady.Value = weapon._canAttack;
            if (weapon.type == Weapon.Type.Melee){
                if (weapon.GetComponent<Melee>().CurrentCombo > 0){
                    inCombo.Value = true;
                }
                else inCombo.Value = false;
            }
        }
    }
}
