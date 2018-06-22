using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace JLProject {
    /// <summary>
    /// transition to the next specified level
    /// </summary>
    public class ExitTrigger_DEBUG : MonoBehaviour{
        public int LevelToLoad;
        void OnTriggerEnter(Collider c) {
            if (c.tag == "Player"){
                DataService.Instance.PlayerStats.UpdateStats(FindObjectOfType<PlayerController>(), LevelToLoad);
                SceneLoader.Instance.LoadLevel(LevelToLoad);
            }
        }
    }
}