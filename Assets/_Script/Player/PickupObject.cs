using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{

    /// <summary>
    /// Script to handle picking up objects on the player's end
    /// </summary>
    public class PickupObject : MonoBehaviour{
        public Transform HoldingRoot;
        private Pickuppable _objToHold;
        public Pickuppable currentObj;
        public float ThrowSpeed = 8.0f;
        public float ArmsLength = 1.5f;
        public bool CanPickUp = false;
        private Ray _fwdRay;
        private RaycastHit _hitInfo;

        void Update(){
            _fwdRay.origin = transform.position;
            _fwdRay.direction = transform.forward;
            if (Physics.Raycast(_fwdRay, out _hitInfo, ArmsLength)){
                _objToHold = _hitInfo.transform.gameObject.GetComponent<Pickuppable>();
                if (_objToHold){
                    CanPickUp = true;
                    _objToHold.Selected = true;
                }
                else{
                    CanPickUp = false;
                }
            }
            else{
                _objToHold = null;
                CanPickUp = false;
            }
        }

        /// <summary>
        /// pick up an object
        /// </summary>
        /// <param name="obj"></param>
        public void PickUp(){
            if (!currentObj) {
                _objToHold.transform.parent = HoldingRoot.transform;
                _objToHold.transform.position = HoldingRoot.position;
                _objToHold.PickUp();
                currentObj = _objToHold;
            }
        }

        /// <summary>
        /// throw the object
        /// </summary>
        public void Release(){
            if (currentObj) {
                currentObj.transform.parent = null;
                currentObj.transform.position = gameObject.transform.position + new Vector3(0, 0.75f, .25f);
                currentObj.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * ThrowSpeed;
                currentObj.SetDamageOn();
                currentObj = null;
            }
        }
    }
}
