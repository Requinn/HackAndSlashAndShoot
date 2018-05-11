using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;
using UnityEngine.AI;

namespace JLProject{
    public abstract class AIEntity : Entity{
        public Weapon weapon;
        public GameObject target;
        protected NavMeshAgent _NMAgent;
        protected FoVDetection _vision;
        protected ImpactReceiver _impact;
        public float mass = 3.0f;
        protected Vector3 _hitDirection;
        private CoroutineHandle knockbackHandle;
        void Awake(){
            _vision = GetComponent<FoVDetection>();
            _NMAgent = GetComponent<NavMeshAgent>();
            target = FindObjectOfType<PlayerController>().gameObject;
        }

        void Start(){
            _impact = GetComponent<ImpactReceiver>();
            if (_impact){
                _impact.mass = mass;
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
