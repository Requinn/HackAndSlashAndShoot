using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using MEC;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace JLProject{
    public class MeleeUnit : AIEntity{
        private FoVDetection _vision;

        public GameObject target;
        public float rotationTime = 3.0f;
        public float movementSpeed = 5.0f;

        private Vector3 _pos, _dir;

        private bool _targetAcquired = false;

        private RaycastHit[] surroundingObjects;

        public PlayMakerFSM thisFSM;
        //unused??
        public State prevState;
        public State state;
        // Use this for initialization
        void Awake(){
            thisFSM = GetComponent<PlayMakerFSM>();
            prevState = state = State.Idle;
            _vision = GetComponent<FoVDetection>();
            _NMAgent = GetComponent<NavMeshAgent>();
            target = FindObjectOfType<PlayerController>().gameObject;
        }

        // Update is called once per frame
        void Update(){
            ProjectileDetection();
        }

        protected void ProjectileDetection(){
            surroundingObjects = Physics.SphereCastAll(transform.position, 4f, Vector3.up);
            foreach (RaycastHit obj in surroundingObjects){
                if (obj.transform.gameObject.tag == "PlayerProjectile" && _canEvade){
                    thisFSM.SendEvent("Evading");
                    break;
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
            weapon.Fire();
        }
    }
}