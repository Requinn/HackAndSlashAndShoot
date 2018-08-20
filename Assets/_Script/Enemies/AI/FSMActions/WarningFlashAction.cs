using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject;
using UnityEngine;

public class WarningFlashAction : FsmStateAction {
    public FsmOwnerDefault gameobject;
    private MeleeUnit _meleeEntity;

    public override void Awake() {
        _meleeEntity = Fsm.GameObject.GetComponent<MeleeUnit>();
    }

    public override void OnUpdate() {
        Flash();
        Finish();
    }

    private void Flash() {
        _meleeEntity.WarningFlash();
    }
}
