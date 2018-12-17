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
        public int nextSceneToLoad = 0;

        void OnTriggerEnter(Collider c){
            if (c.tag == "Player"){
                if (nextSceneToLoad == 0) {
                    SceneLoader.Instance.LoadNextLevel();
                }
                else {
                    SceneLoader.Instance.LoadLevel(nextSceneToLoad);
                }
            }
        }
    }
}
