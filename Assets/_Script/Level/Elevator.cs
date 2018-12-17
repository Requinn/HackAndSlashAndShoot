using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using UnityEngine;

namespace JLProject{
    public class Elevator : MonoBehaviour{
        public float velocity;
        public bool oneWay = false;
        public Transform pointA, pointB;
        public TranslateObject doorA, doorB;
        public GameObject platformDoorA, platformDoorB;
        public Toggleable elevatorSwitch;
        private Vector3 _destinationPosition;
        private bool _occupied, _inTransit, _locked = false;

        void Start(){
            _destinationPosition = transform.position;
        }

        // Update is called once per frame
        void Update(){
            if (_locked) {
                return;
            }
            // movement of the elevator
            if(!V3Equal(transform.position, _destinationPosition)) {
                transform.position = Vector3.MoveTowards(transform.position, _destinationPosition, Time.smoothDeltaTime * velocity);
                //Reached destination
                if (V3Equal(transform.position,_destinationPosition)){
                    if (V3Equal(transform.position, pointA.position)) {
                        elevatorSwitch.Toggle();
                        if (platformDoorA) platformDoorA.SetActive(false);
                        doorA.Invoke("Open", 0.25f);
                    }
                    if (V3Equal(transform.position, pointB.position)){
                        if (platformDoorB) platformDoorB.SetActive(false);
                        doorB.Invoke("Open", 0.25f);
                        elevatorSwitch.Toggle();
                    }
                    //if this elevator is one way we disable the collider
                    if (oneWay) {
                        _locked = true;
                    }
                    //delay the switch retoggle to avoid the doors breaking
                    Timing.RunCoroutine(ElevatorSafetyTimer());
                }
            } 
            //input and destination checking, only moves when we have both destinations set
            if (_occupied && Input.GetKeyDown(KeyCode.E) && !_inTransit && (pointA && pointB)){
                _inTransit = true;
                elevatorSwitch.Toggle();
                if (V3Equal(transform.position, pointB.position)) {  
                    if (platformDoorB) platformDoorB.SetActive(true);
                    doorB.Close();
                    Timing.RunCoroutine(ChangeDirection(pointA.position));
                }
                if (V3Equal(transform.position, pointA.position)) {
                    if (platformDoorA) platformDoorA.SetActive(true);
                    doorA.Close();
                    Timing.RunCoroutine(ChangeDirection(pointB.position));
                }
            }
        }

        private IEnumerator<float> ElevatorSafetyTimer() {
            yield return Timing.WaitForSeconds(0.35f);
            _inTransit = false;
        }

        public IEnumerator<float> ChangeDirection(Vector3 newPosition){
            yield return Timing.WaitForSeconds(0.25f);
            _destinationPosition = newPosition;
        }

        public Vector3 GetPosition(){
            return transform.position;
        }

        void OnTriggerEnter(Collider c){
            if (!_locked && c.CompareTag("Player")){
                c.transform.SetParent(transform, true);
                if ((pointA && pointB)) {
                    elevatorSwitch.Toggle();
                }
                _occupied = true;
            }
        }

        void OnTriggerExit(Collider c){
            if (!_locked && c.CompareTag("Player")){
                c.transform.parent = null;
                if ((pointA && pointB)) {
                    elevatorSwitch.Toggle();
                }
                _occupied = false;
            }
        }

        private bool V3Equal(Vector3 a, Vector3 b) {
            return Vector3.SqrMagnitude(a - b) < 0.0001;
        }
    }
}
