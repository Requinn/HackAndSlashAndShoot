﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public class CameraController : MonoBehaviour{
        private Transform _player;
        public float CameraHeight = 9.0f, CameraHorizontal = 0.0f, CameraVertical = -7.5f, XAngle = 45f;
        public bool LockedToPlayer = true;
        public float CameraFollowDelay = 0.1f; //How behind the camera is when the player moves
        private float _screenHeight, _screenWidth, _cameraHeight, _cameraTimetoReach;
        private Vector3 _target;
        private Vector3 _camVelocity = Vector3.zero;

        public bool FadeObstruction = true;
        private RaycastHit _lineHit;
        private ObjectCameraFader _objFader;
        // Use this for initialization
        void Awake(){
            _player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
            _cameraTimetoReach = CameraFollowDelay;
            transform.rotation = Quaternion.Euler(XAngle, 0, 0);
        }
        // Update is called once per frame
        void Update(){
            //camera following the player
            if (LockedToPlayer && _player){
                _target = _player.position + new Vector3(CameraHorizontal, CameraHeight, CameraVertical);
            }
            transform.position = Vector3.SmoothDamp(transform.position, _target, ref _camVelocity, _cameraTimetoReach);

            if (FadeObstruction){
                FadeObjectBetweenPlayer();
            }
           
        }

        private void FadeObjectBetweenPlayer(){
            if (Physics.Linecast(transform.position, _player.position, out _lineHit)){
                _objFader = _lineHit.transform.GetComponent<ObjectCameraFader>();
                if (_objFader){
                    _objFader.EnableFade();
                }
            }
        }

        //Moves the camera to the targetPos in timeToReach seconds
        public void MoveCamera(Transform targetPos, float timeToReach){
            _target = targetPos.position;
            _cameraTimetoReach = timeToReach;
            LockedToPlayer = false;
        }

        //Moves the camera back onto focusing the player
        public void RelockCamera(){
            LockedToPlayer = true;
            _cameraTimetoReach = CameraFollowDelay;
        }
    }
}