using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace JLProject {
    public class HeavySmasher : AIEntity{
        private FoVDetection _vision;

        public GameObject target;
        public float rotationTime = 3.0f;
        public float movementSpeed = 5.0f;

        private Vector3 _pos, _dir;

        public State prevState;

        public State state;

        // Use this for initialization
        void Awake(){
            prevState = state = State.Idle;
            _vision = GetComponent<FoVDetection>();
            _NMAgent = GetComponent<NavMeshAgent>();
            target = FindObjectOfType<PlayerController>().gameObject;
        }

        // Update is called once per frame
        void Update(){
        }

        /// <summary>
        /// called from the FSM
        /// </summary>
        protected override void Movement(){
            Rotate();
            if (Vector3.Distance(transform.position, target.transform.position) > _vision.maxAttackRange){
                _NMAgent.Move(transform.forward * movementSpeed * Time.deltaTime);
            }
        }

        private Quaternion _lookrotation;

        protected void Rotate(){
            _pos = transform.position;
            _dir = (target.transform.position - _pos).normalized; //direction to look at
            _lookrotation = Quaternion.LookRotation(_dir); //generate a quaternion using the direction
            transform.DORotate(_lookrotation.eulerAngles, rotationTime); //rotate towards it with a speed
        }

        protected override void Attack(){
            weapon.Fire();
        }

        public override void ProjectileResponse(){
        }
    }
}