using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace JLProject {
    public class ProjectileResponseAction : FsmStateAction {
        public FsmOwnerDefault gameobject;
        private AIEntity entity;
        public override void Awake() {
            entity = Fsm.GameObject.GetComponent<AIEntity>();
        }

        public override void OnUpdate() {
            Response();
            Finish();
        }

        private void Response(){
            entity.ProjectileResponse();
        }

    }
}