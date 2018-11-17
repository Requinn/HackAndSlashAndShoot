using System.Collections;
using System.Collections.Generic;
using JLProject;
using JLProject.Spline;
using UnityEngine;

public class PayloadObjective : LevelObjective {
    public override void Initiate(){
        throw new System.NotImplementedException();
    }

    public PayloadObject payload;

    void Start(){
        payload.EndReached += CompleteObjective;
    }
    //listen for payload object's completion

    private void CompleteObjective(){
        if (ObjectToUnlock) {
            ObjectToUnlock.Open();
        }
    }
}
