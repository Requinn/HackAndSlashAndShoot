using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using MEC;
using UnityEngine;
using UnityEngine.AI;

namespace JLProject{
    public class MeleeUnit : AIEntity{
        public float rotationTime = 3.0f;
        public float movementSpeed = 5.0f;
        protected float speed;
        private Vector3 _pos, _dir;

        protected RaycastHit[] surroundingObjects;

        public PlayMakerFSM thisFSM;
        //unused??
        public State prevState;
        public State state;
        // Use this for initialization
        void Start(){
            speed = movementSpeed;
            thisFSM = GetComponent<PlayMakerFSM>();
            prevState = state = State.Idle;
            
        }

        /// <summary>
        /// called from the FSM
        /// </summary>
        protected override void Movement(){
            if (_vision.inRange){
                Rotate();
                if (Vector3.Distance(transform.position, target.transform.position) > _vision.maxAttackRange){
                    _NMAgent.Move(transform.forward * speed * Time.deltaTime);
                }
            }
        }

        private Quaternion _lookrotation;
        protected void Rotate(){
            _pos = transform.position;
            _dir = (target.transform.position - _pos).normalized;       //direction to look at
            _lookrotation = Quaternion.LookRotation(_dir);              //generate a quaternion using the direction
            transform.DORotate(_lookrotation.eulerAngles, rotationTime);    //rotate towards it with a speed
        }
        protected override void Attack(){
            weapon.Fire();
        }

        public override void ProjectileResponse() {
        }
    }
}