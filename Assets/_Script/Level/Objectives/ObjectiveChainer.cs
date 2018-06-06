using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// Serves as a bridge to chain objective together
    /// i.e. a switch hit into an ambusj
    /// </summary>
    public class ObjectiveChainer : MonoBehaviour{
        public LevelObjective TriggeringObjective;
        public LevelObjective ObjectiveToStart;

        void Start(){
            TriggeringObjective.OnCompleteObjective += ObjectiveToStart.Initiate;
        }
    }
}
