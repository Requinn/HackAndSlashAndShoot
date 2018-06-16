using UnityEngine;
using MEC;
using System.Collections.Generic;

namespace JLProject {
    /// <summary>
    /// simple switch to open and close door(s)
    /// </summary>
    public class Switch : LevelObjective {
        //a redundancy: this is only used by switched to handle multiple toggled objects
        public Toggleable[] Toggleables;
        public bool on = false;
        public bool canToggle = true;
        [SerializeField]
        private float toggleSafetyTime = 0.5f;//can only toggle once per second

        public virtual void Toggle(){
            if (!canToggle) return;
            //toggle everything like doors
            foreach (var T in Toggleables){
                T.Toggle();
            }

            on = !on; //flip on

            //tell our objective that we did our thing
            if (!isObjectiveComplete) {
                OnCompleteObjective();
                isObjectiveComplete = true;
            }

            //check if we have to go into cooldown
            if (oneWay){
                canToggle = false;
            }
            else{
                Timing.RunCoroutine(ToggleDelay());
            }
        }

        public IEnumerator<float> ToggleDelay() {
            canToggle = false;
            yield return Timing.WaitForSeconds(toggleSafetyTime);
            canToggle = true;
        }

        public override void Initiate(){
            //do nothing
        }
    }
}