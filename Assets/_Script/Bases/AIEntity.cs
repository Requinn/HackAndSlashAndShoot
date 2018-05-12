using System.Collections;
using System.Collections.Generic;
using JLProject;
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
            OnDeath += PlayDeathAnimation;
            OnDeath += SetSelfInactive;
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
        /// handles knockback on enabled entities 
        /// experimental
        /// </summary>
        /// <param name="args"></param>
        private void KnockBack(Damage.DamageEventArgs args){
            _hitDirection = (transform.position - args.HitSourceLocation).normalized;
            _NMAgent.velocity = _hitDirection * args.HitForce;
            Timing.KillCoroutines(knockbackHandle);
            knockbackHandle = Timing.RunCoroutine(KnockBackCo());
        }

        private IEnumerator<float> KnockBackCo(){
            _NMAgent.speed = 10;
            _NMAgent.angularSpeed = 0;//Keeps the enemy facing forwad rther than spinning
            _NMAgent.acceleration = 20;

            yield return Timing.WaitForSeconds(0.2f);//Only knock the enemy back for a short time    

            _NMAgent.speed = 4;
            _NMAgent.angularSpeed = 180;
            _NMAgent.acceleration = 10;
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
