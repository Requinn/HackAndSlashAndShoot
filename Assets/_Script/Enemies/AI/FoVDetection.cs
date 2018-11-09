using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace JLProject{
    public class FoVDetection : MonoBehaviour{
        [Tooltip("Max view range.")] [SerializeField] private float _maxViewRange;
        [Tooltip("Max attacking distance")] public float maxAttackRange;
        [Range(0, 180)] [SerializeField] private float _xAngle = 60.0f; //viewing cone
        [Range(0, 180)] [SerializeField] private float _yAngle = 30.0f;
        [Range(0, 180)] [SerializeField] private float _aggroXAngle = 15.0f; //attacking cone

        public Vector3 lastSeenPosition;

        public bool inRange = false;
        public bool inAttackCone = false;
        public bool targetFound = false;
        private Transform _eyeTransform;
        private float _distanceToTarget;
        
        /// <summary>
        /// Get the distance to the current Target being looked at
        /// </summary>
        public float GetDistanceToTarget { get { return _distanceToTarget; } }
        // Use this for initialization
        void Awake(){
            _eyeTransform = transform;
        }

        void Update(){
        }

        /// <summary>
        /// Field of View detection 
        /// Thanks to Michael W. for a much better version of this
        /// TODO CLEAN UP THIS MESS
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CanSeeTarget(Transform target){
            if (target == null) return false; //target doesn't exist
            var sightDistance = _eyeTransform.position - target.position;
            _distanceToTarget = sightDistance.magnitude; //using a magnitude, if performance dips look here
            //debugstr = "";
            //within view range
            if (_distanceToTarget < _maxViewRange){
                //debugstr += "Within Range > ";
                inRange = true;
                //check if the target is behind us
                if (_eyeTransform.InverseTransformPoint(target.position).z < 0.0f){
                    //if they're behind us return true
                    if (inRange) {
                        inAttackCone = false;
                        return true;
                    }
                }

                var targetRelativeToEye = _eyeTransform.InverseTransformPoint(target.position);
                var projectedVector = Vector3.Project(targetRelativeToEye, Vector3.forward);
                //start checking for the angle in the X plane
                if (projectedVector.magnitude > 0.0f){
                    //debugstr += "In front > ";
                    float angle = Vector3.Angle(target.position - transform.position, transform.forward);
                    if (angle <= _xAngle){
                        //debugstr += "In Cone > ";
                        if (angle <= _aggroXAngle && sightDistance.magnitude < maxAttackRange){
                            inAttackCone = true;
                        }
                        else{
                            inAttackCone = false;
                        }
                        RaycastHit hit;
                        //Layermask to ignore raycasting the projectile layer ~(1 << 9), doesn't seem to work
                        if (Physics.Raycast(_eyeTransform.position, -sightDistance, out hit, _maxViewRange )){
                            //debugstr += "FOUND";
                            //Debug.DrawRay(_eyeTransform.position, -sightDistance, Color.cyan, 600.0f);
                            //Debug.Log(hit.collider.gameObject);
                            if (hit.collider.gameObject.tag == target.tag){
                                lastSeenPosition = hit.collider.gameObject.transform.position;
                                return true;
                            }
                        }
                    }
                }
            }
            inAttackCone = false;
            inRange = false;
            return false;
        }
    }
}
