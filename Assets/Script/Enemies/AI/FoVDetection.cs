using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class FoVDetection : MonoBehaviour{
    [Tooltip("Max view range.")] [SerializeField] private float _maxRange;

    [Range(0, 180)] [SerializeField] private float _xAngle = 60.0f;
    [Range(0, 180)] [SerializeField] private float _yAngle = 30.0f;

    //public string debugstr;
    public bool drawBounds = false; 
    private Transform _eyeTransform;
	// Use this for initialization
	void Awake (){
	    _eyeTransform = transform;
	}

    void Update(){
        if (drawBounds){
            DrawBounds();
        }
    }
    /// <summary>
    /// Field of View detection 
    /// Thanks to Michael W. for a much better version of this
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool CanSeeTarget(Transform target){
        if (target == null) return false; //target doesn't exist
        var sightDistance = _eyeTransform.position - target.position;
        //debugstr = "";
        //within view range
        if (sightDistance.magnitude < _maxRange){
            //debugstr += "Within Range > ";
            //check if the target is behind us
            if (_eyeTransform.InverseTransformPoint(target.position).z < 0.0f) return false;

            var targetRelativeToEye = _eyeTransform.InverseTransformPoint(target.position);
            var projectedVector = Vector3.Project(targetRelativeToEye, Vector3.forward);
            //start checking for the angle in the X plane
            if (projectedVector.magnitude > 0.0f){
                //debugstr += "In front > ";
                if (Vector3.Angle(target.position - transform.position, transform.forward) <= _xAngle){
                    //debugstr += "In Cone > ";
                    RaycastHit hit;
                    if (Physics.Raycast(_eyeTransform.position, -sightDistance, out hit, _maxRange)){
                        //debugstr += "FOUND";
                        Debug.DrawRay(_eyeTransform.position, -sightDistance, Color.cyan, 600.0f);
                        Debug.Log(hit.transform.gameObject);
                        if (hit.collider.gameObject.tag == target.tag){
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private void DrawBounds() {
        var boundsColor = Color.red;
        var center = _eyeTransform.position + (_eyeTransform.forward * _maxRange);
        var endCorner1 = center - (_eyeTransform.up * (_maxRange)) +
                         (_eyeTransform.right * (_maxRange));
        var endCorner2 = center + (_eyeTransform.up * (_maxRange)) +
                         (_eyeTransform.right * (_maxRange));
        var endCorner3 = center + (_eyeTransform.up * (_maxRange)) -
                         (_eyeTransform.right * (_maxRange));
        var endCorner4 = center - (_eyeTransform.up * ( _maxRange)) -
                         (_eyeTransform.right * _maxRange);
        Debug.DrawLine(_eyeTransform.position, endCorner1, boundsColor);
        Debug.DrawLine(_eyeTransform.position, endCorner2, boundsColor);
        Debug.DrawLine(_eyeTransform.position, endCorner3, boundsColor);
        Debug.DrawLine(_eyeTransform.position, endCorner4, boundsColor);
        Debug.DrawLine(endCorner1, endCorner2, boundsColor);
        Debug.DrawLine(endCorner2, endCorner3, boundsColor);
        Debug.DrawLine(endCorner3, endCorner4, boundsColor);
        Debug.DrawLine(endCorner4, endCorner1, boundsColor);
    }
}
