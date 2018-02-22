using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// supposed to go to the next level or whatever, restart level for now
/// </summary>
namespace JLProject{
    public class ExitTrigger : MonoBehaviour{
        void OnTriggerEnter(Collider c){
            GameController.Controller.RestartLevel();
        }
    }
}
