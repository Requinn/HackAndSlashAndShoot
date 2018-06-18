using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject.Weapons;
using MEC;
using UnityEngine;

namespace JLProject{
    public class MultiBossController : AIEntity{
        public float attackDelay = 3.0f; //how often we fire off an attack
        public Weapon[] BossWeapons;
        private float tempArmor;
        public CoroutineHandle attackHandle;
        void OnEnable(){
            tempArmor = ArmorValue;
            ArmorValue = 999f;
            UIhp.gameObject.SetActive(true);
        }

        public void Initiate(){
            ArmorValue = tempArmor;
            if (CurrentHealth == 1f){
                HandleRevive();
            }
            attackHandle = Timing.RunCoroutine(AttackCycle());
        }

        private IEnumerator<float> AttackCycle(){
            while (CurrentHealth > 0){
                yield return Timing.WaitForSeconds(attackDelay);
                BossWeapons[0].Fire();
                yield return Timing.WaitForSeconds(attackDelay);
                BossWeapons[1].Fire();
            }
            yield return 0f;
        }

        private void FireBombard(){
            Debug.Log("BOMB");
        }

        private void FireBeam(){
            Debug.Log("BEAAM");
        }

        
        protected override void HandleDeath(){
            IsDead = true;
            CurrentHealth = 1;           
            ArmorValue = 999f;
            OnDeath();
        }

        protected override void Movement(){
            throw new System.NotImplementedException();
        }

        protected override void Attack(){
            throw new System.NotImplementedException();
        }

        public override void ProjectileResponse(){
            throw new System.NotImplementedException();
        }
    }
}
