using System.Collections;
using System.Collections.Generic;
using JLProject;
using JLProject.Weapons;
using MEC;
using UnityEngine;
using UnityEngine.AI;

namespace JLProject{
    public abstract class AIEntity : Entity{
        public AIAnimationController AnimController;
        public Weapon weapon;
        public GameObject target;
        protected NavMeshAgent _NMAgent;
        protected FoVDetection _vision;
        protected ImpactReceiver _impact;
        public float mass = 3.0f;
        protected Vector3 _hitDirection;
        private CoroutineHandle knockbackHandle;
        protected CharacterController _CC;
        public PlayMakerFSM thisFSM;

        void Awake(){
            thisFSM = GetComponent<PlayMakerFSM>();
            _CC = GetComponent<CharacterController>();
            _vision = GetComponent<FoVDetection>();
            _NMAgent = GetComponent<NavMeshAgent>();
            target = FindObjectOfType<PlayerController>().gameObject;
            
        }

        protected void Start(){
            _impact = GetComponent<ImpactReceiver>();
            if (_impact){
                _impact.mass = mass;
            }
            TookDamage += PlayHitAnimation;
            TookDamage += AggroAttacker;
            OnDeath += PlayDeathAnimation;
            OnDeath += SetSelfInactive;
        }

        protected void AggroAttacker(Damage.DamageEventArgs args) {
            if (args.SourceFaction == Damage.Faction.Player) {
                //Debug.Log("Hit by player");
            }
        }

        private void SetSelfInactive() {
            if(_NMAgent) _NMAgent.enabled = false;
            if(_CC) _CC.enabled = false;
            if(thisFSM) thisFSM.enabled = false;
            enabled = false;
        }

        protected void PlayDeathAnimation(){
            if (AnimController){
                AnimController.PlayAnim("Death");
            }
        }

        protected void PlayHitAnimation(Damage.DamageEventArgs args){
            if (AnimController){
                AnimController.PlayAnim("Hit");
            }
        }

        /// <summary>
        /// this is for fsm experiments
        /// </summary>
        public void Move(){
            Movement();
        }

        public abstract void ProjectileResponse();
    }
}
