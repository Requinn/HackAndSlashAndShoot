using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject.Weapons;
using UnityEngine;

namespace JLProject{
    public class EnemyAttackAction : FsmStateAction{
        public FsmOwnerDefault gameobject;
        private AIEntity entity;

        public override void Awake(){
            entity = Fsm.GetOwnerDefaultTarget(gameobject).GetComponentInChildren<AIEntity>();
        }

        public override void OnEnter() {
            //weapon = Fsm.GetOwnerDefaultTarget(gameobject).GetComponentInChildren<Weapon>();
            Attack();
            Finish();
        }

        public override void OnUpdate() {
            
        }

        public void Attack(){
            if (entity.weapon._canAttack){
                entity.weapon.Fire();
            }
        }
    }
}
