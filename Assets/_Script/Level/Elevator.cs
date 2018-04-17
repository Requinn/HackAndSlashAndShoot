﻿using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace JLProject{
    public class Elevator : MonoBehaviour{
        public float velocity;
        public Transform pointA, pointB;
        public VerticalDoor doorA, doorB;
        public GameObject platformDoorA, platformDoorB;
        private Vector3 _destinationPosition;
        private bool _occupied, _inTransit;

        void Start(){
            _destinationPosition = transform.position;
        }

        // Update is called once per frame
        void Update(){
            if(!V3Equal(transform.position, _destinationPosition)) {
                transform.position = Vector3.MoveTowards(transform.position, _destinationPosition, Time.smoothDeltaTime * velocity);
                if (V3Equal(transform.position,_destinationPosition)){
                    if (V3Equal(transform.position, pointA.position)) {
                        if(platformDoorA) platformDoorA.SetActive(false);
                        doorA.Invoke("Open", 0.25f);
                    }
                    if (V3Equal(transform.position, pointB.position)){
                        if(platformDoorB) platformDoorB.SetActive(false);
                        doorB.Invoke("Open", 0.25f);
                    }
                    _inTransit = false;
                }
            } 
            if (_occupied && Input.GetKeyDown(KeyCode.E) && !_inTransit){
                if (V3Equal(transform.position, pointB.position)) {
                    if(platformDoorB) platformDoorB.SetActive(true);
                    doorB.Close();
                    Timing.RunCoroutine(ChangeDirection(pointA.position));
                }
                if (V3Equal(transform.position, pointA.position)) {
                    if(platformDoorA) platformDoorA.SetActive(true);
                    doorA.Close();
                    Timing.RunCoroutine(ChangeDirection(pointB.position));
                }
            }
        }

        public IEnumerator<float> ChangeDirection(Vector3 newPosition){
            yield return Timing.WaitForSeconds(0.25f);
            _destinationPosition = newPosition;
            _inTransit = true;
        }

        public Vector3 GetPosition(){
            return transform.position;
        }

        void OnTriggerEnter(Collider c){
            if (c.CompareTag("Player")){
                c.transform.SetParent(transform, true);
                _occupied = true;
            }
        }

        void OnTriggerExit(Collider c){
            if (c.CompareTag("Player")){
                c.transform.parent = null;
                _occupied = false;
            }
        }

        private bool V3Equal(Vector3 a, Vector3 b) {
            return Vector3.SqrMagnitude(a - b) < 0.0001;
        }
    }
}
