using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace JLProject{
    public class EnemyAttackAction : FsmStateAction{
        public FsmOwnerDefault gameobject;
        private Weapon weapon;

        public override void Awake(){
            weapon = Fsm.GetOwnerDefaultTarget(gameobject).GetComponentInChildren<Weapon>();
        }

        public override void OnEnter() {
            //weapon = Fsm.GetOwnerDefaultTarget(gameobject).GetComponentInChildren<Weapon>();
            Attack();
        }

        public override void OnUpdate() {
            
        }

        public void Attack(){
            if (weapon._canAttack){
                weapon.Fire();
            }
        }
    }
}
