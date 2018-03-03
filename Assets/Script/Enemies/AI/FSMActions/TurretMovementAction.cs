using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace JLProject {
    public class TurretMovementAction : FsmStateAction {
        public FsmOwnerDefault gameobject;
        private Weapon weapon;
        private Turret turret;
        public override void Awake(){
            turret = Fsm.GetOwnerDefaultTarget(gameobject).GetComponent<Turret>();

        }

        public override void OnUpdate() {
            turret.Move();
            Finish();
        }

    }
}