using System;
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
        public SkinnedMeshRenderer _enemyMesh;
        private Color _originalMeshColor;
        protected RaycastHit[] surroundingObjects;
        private CoroutineHandle _postChaseScanRoutine;

        // Use this for initialization
        protected new void Start(){
            base.Start();
            //_enemyMesh = GetComponent<Renderer>().material;
            if (_enemyMesh){
                _originalMeshColor = _enemyMesh.material.color;
            }
            OnDeath += StopRoutine;
            speed = movementSpeed;
        }

        private void StopRoutine() {
            Timing.KillCoroutines(_postChaseScanRoutine);
        }

        /// <summary>
        /// called from the FSM
        /// </summary>
        protected override void Movement(){
            if (!IsDead) {
                if (_vision.inRange) {
                    Timing.KillCoroutines(_postChaseScanRoutine);
                    _NMAgent.ResetPath();
                    Rotate();
                    if (Vector3.Distance(transform.position, target.transform.position) > _vision.maxAttackRange) {
                        if (AnimController) AnimController.WalkForward();
                        _NMAgent.Move(transform.forward * speed * Time.deltaTime);
                    }
                }
                else {
                    //if we lose sight, go to where we last saw them
                    if (_vision.lastSeenPosition != Vector3.zero) {
                        _NMAgent.SetDestination(_vision.lastSeenPosition);
                        _postChaseScanRoutine = Timing.RunCoroutine(CheckForScanLocation());
                    }
                    else {
                        if (AnimController) AnimController.Idle();
                    }
                }
            }
        }
        
        /// <summary>
        /// checking constantly if we reached the last seen position when we lose sight. 
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CheckForScanLocation() {
            while(!IsDead && transform.position != _NMAgent.destination) {
                if (gameObject == null) {
                    yield break;
                }
                yield return 0f;
            }
            _postChaseScanRoutine = Timing.RunCoroutine(WaitInPlace());
        }

        /// <summary>
        /// Wait a while in place
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> WaitInPlace() {
            yield return Timing.WaitForSeconds(UnityEngine.Random.Range(1f, 2.5f));
            _vision.lastSeenPosition = Vector3.zero;
        }


        /// <summary>
        /// Perform a quick flash before attacking
        /// </summary>
        public void WarningFlash(){
            if (_enemyMesh){
                Timing.RunCoroutine(FlashRoutine());
            }

        }

        private IEnumerator<float> FlashRoutine(){
            float elapsedTime = 0f;
            while (elapsedTime < 0.1f){
                _enemyMesh.material.color = Color.red;
                yield return Timing.WaitForSeconds(0.06f);
                _enemyMesh.material.color = Color.white;
                yield return Timing.WaitForSeconds(0.06f);
                elapsedTime += Time.deltaTime;
            }

            yield return 0f;
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