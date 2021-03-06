﻿using System;
using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;
using JLProject.Spline;
using MEC;

/// <summary>
/// Script that will be moved along a spline and inform the payloadobjective when its progress hits 1f
/// </summary>
public class PayloadObject : MonoBehaviour{
    //Have an array of objective stuff (that will wrap things like switches) that this will subscribe events to and allow halting/continuation
    //another array of values that will be the progress this payload stops at
    [SerializeField]
    private GameObject _payloadToMove; //keep this null if you are attaching this to something instead
    public float activeDistance = 5.0f;
    public float timeToReachEnd = 80f;
    public BezierSpline rail;
    [SerializeField]
    private float progress;
    private GameObject _playerObject;
    private bool _completed = false;
    private bool _halted = false;
    public delegate void OnCompleteEvent();
    public event OnCompleteEvent EndReached;

    public LevelObjective[] Objectives; //what is the event we want to happen
    public float[] ObjectiveProgressThreshold; //used to mark where in our progress an event will happen
    //values for the payload level for later: 0.083 (first gate), 0.21 (ambush, second gate), 0.35 (boardwalk) , 0.57 (docks), 0.75 (boat)
    private int _objectiveIndex = 0;

    void Start(){
        _playerObject = FindObjectOfType<PlayerController>().gameObject;
        if (!_payloadToMove) {
            _payloadToMove = gameObject;
        }
        _payloadToMove.transform.position = rail.GetPoint(0f);

        foreach (var o in Objectives){
            o.OnCompleteObjective += ResumeProgress;
        }
    }

    void ResumeProgress(){
        _halted = false;
    }

    void HaltProgress(){
        _halted = true;
    }

    void Update(){
        if (!_completed){
            if (!_halted){
                //check we're in range to push the cart
                if (Vector3.SqrMagnitude(_playerObject.transform.position - _payloadToMove.transform.position) <
                    activeDistance * activeDistance){
                    //if we aren't at the, move along the splin
                    if (progress < 1f){
                        progress += Time.deltaTime / timeToReachEnd;
                        Vector3 position = rail.GetPoint(progress);
                        //if we still have stuff to do, check if we are at that location
                        if (_objectiveIndex < ObjectiveProgressThreshold.Length){
                            if (Math.Abs(progress - ObjectiveProgressThreshold[_objectiveIndex]) < 0.001){
                                if (!Objectives[_objectiveIndex].isCompleted){
                                    Objectives[_objectiveIndex].Initiate();
                                    HaltProgress();
                                }
                                _objectiveIndex++;
                            }
                        }
                        _payloadToMove.transform.localPosition = position;
                        _payloadToMove.transform.LookAt(position + rail.GetDirection(progress));
                    }
                    //heeyy we made it
                    else{
                        EndReached();
                        _completed = true;
                    }
                }
            }
        }
    }
}
