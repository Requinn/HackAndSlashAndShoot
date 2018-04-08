using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace JLProject {
    public class EvasiveAction : FsmStateAction {
        public FsmOwnerDefault gameobject;
        private AIEntity entity;
        public override void Awake() {
            entity = Fsm.GameObject.GetComponent<AIEntity>();
        }

        public override void OnUpdate() {
            Evade();
            Finish();
        }

        private void Evade() {
            if (entity._canEvade){
                if (Random.Range(0, 2) == 1){
                    MEC.Timing.RunCoroutine(entity.Evade(-entity.transform.right));
                }
                else{
                    MEC.Timing.RunCoroutine(entity.Evade(entity.transform.right));
                }
            }
        }

    }
}