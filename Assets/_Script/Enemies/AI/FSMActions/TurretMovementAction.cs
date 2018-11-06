using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject.Weapons;
using UnityEngine;
using DG.Tweening;

namespace JLProject {
    /// <summary>
    /// An FSM Action to mimic turret like movement through pure rotations
    /// </summary>
    public class TurretMovementAction : FsmStateAction {
        public FsmOwnerDefault gameobject;
        private Vector3 _pos, _dir;
        private Quaternion _lookrotation;
        private float rotationTime = 0.1f;
        private Transform transform;
        private GameObject target;

        public override void Awake(){
            transform = Fsm.GameObject.transform;
        }

        public override void OnUpdate() {
            target = GameController.Controller.PlayerReference.gameObject;
            _pos = transform.position;
            _dir = (target.transform.position - _pos).normalized;       //direction to look at
            _lookrotation = Quaternion.LookRotation(_dir);              //generate a quaternion using the direction
            transform.DORotate(_lookrotation.eulerAngles, rotationTime);    //rotate towards it with a speed
            Finish();
        }

    }
}