using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace JLProject {
    public class MovementAction : FsmStateAction {
        public FsmOwnerDefault gameobject;
        private Weapon weapon;
        private MeleeRusher entity;
        public override void Awake(){
            entity = Fsm.GameObject.GetComponent<MeleeRusher>();
        }

        public override void OnUpdate(){
            Move();
            Finish();
        }

        private void Move(){
            entity.Move();
        }

    }
}