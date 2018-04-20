using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// transition to the next level
    /// </summary>
    public class ExitTrigger : MonoBehaviour{

        void OnTriggerEnter(Collider c){
            if (c.tag == "Player"){
                SceneLoader.Instance.LoadNextLevel();
            }
        }
    }
}
