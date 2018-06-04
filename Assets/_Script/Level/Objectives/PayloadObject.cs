﻿using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;
using JLProject.Spline;

/// <summary>
/// Script that will be moved along a spline and inform the payloadobjective when its progress hits 1f
/// </summary>
public class PayloadObject : MonoBehaviour{
    public float activeDistance = 5.0f;
    public float timeToReachEnd = 80f;
    public BezierSpline rail;
    private float progress;
    private GameObject _playerObject;

    public delegate void OnCompleteEvent();
    public event OnCompleteEvent EndReached;

    void Start(){
        _playerObject = FindObjectOfType<PlayerController>().gameObject;
        transform.position = rail.GetPoint(0f);
    }

    void Update(){
        if (Vector3.SqrMagnitude(_playerObject.transform.position - transform.position) < activeDistance * activeDistance) {
            if (progress < 1f){
                progress += Time.deltaTime / timeToReachEnd;
                Vector3 position = rail.GetPoint(progress);
                transform.localPosition = position;
                transform.LookAt(position + rail.GetDirection(progress));
            }
            else{
                EndReached();
            }
        }
        
    }
    //do an event on progress == 1f
}
