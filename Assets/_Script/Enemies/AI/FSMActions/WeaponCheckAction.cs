using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject;
using JLProject.Weapons;
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
            if (weapon.damageType == Weapon.Type.Melee){
                Melee m = weapon.GetComponent<Melee>();
                if (m.CurrentCombo > 0) {
                    inCombo.Value = true;
                }
                else { inCombo.Value = false; }
            }
        }
    }
}
