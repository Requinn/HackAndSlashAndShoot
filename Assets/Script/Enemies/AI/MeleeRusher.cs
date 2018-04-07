using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace JLProject{
    public class MeleeRusher : Entity{
        public bool useAI; //DEBUG ONLY
        private FoVDetection _vision;

        public GameObject target;
        public float rotationTime = 3.0f;
        public float movementSpeed = 5.0f;

        private Vector3 _pos, _dir;
        public Weapon weapon;

        private bool _targetAcquired = false;
        private NavMeshAgent _NMAgent;

        // Use this for initialization
        void Awake(){
            _vision = GetComponent<FoVDetection>();
            _NMAgent = GetComponent<NavMeshAgent>();
            target = FindObjectOfType<PlayerController>().gameObject;
        }

        // Update is called once per frame
        void Update(){
            if (useAI){
                if (_vision.CanSeeTarget(target.transform)){
                    if (_vision.inAttackCone){
                        Attack();
                    }
                    else if(Vector3.Distance(transform.position, target.transform.position) > 1f){
                        Movement();
                    }
                }
            }
        }
        
        /// <summary>
        /// called from the FSM
        /// </summary>
        protected override void Movement(){
            Rotate();
            if(Vector3.Distance(transform.position, target.transform.position) > _vision.maxAttackRange){
                _NMAgent.Move(transform.forward * movementSpeed * Time.deltaTime);
            }
        }

        private Quaternion _lookrotation;
        protected void Rotate(){
            _pos = transform.position;
            _dir = (target.transform.position - _pos).normalized;       //direction to look at
            _lookrotation = Quaternion.LookRotation(_dir);              //generate a quaternion using the direction
            transform.DORotate(_lookrotation.eulerAngles, rotationTime);    //rotate towards it with a speed
        }
        /// <summary>
        /// this is for fsm experiments
        /// </summary>
        public void Move() {
            Movement();
        }
        protected override void Attack(){
            if (weapon._canAttack){
                weapon.Fire();
            }
        }
    }
}