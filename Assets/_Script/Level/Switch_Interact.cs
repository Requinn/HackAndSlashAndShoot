using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// This is code for a switch that has to be interacted with
    /// </summary>
    public class Switch_Interact : LevelObjective{
        public Toggleable[] Toggleables;
        [SerializeField] private MaterialToggle materialSwitch;
        private bool on = false;
        public bool canToggle = true;
        private bool _occupied;
        [SerializeField] private float toggleSafetyTime = 0.5f;//can only toggle once per second

        public virtual void Toggle() {
            if (!canToggle) return;
            //toggle all the stuff we should be like doors
            foreach (var T in Toggleables) {
                T.Toggle();
            }
            //flip on 
            on = !on;

            //if we're part of an objective, mark as done
            if (!isObjectiveComplete) {
                OnCompleteObjective();
                isObjectiveComplete = true;
            }

            //check do we go into cooldown or are we done here
            if (oneWay) {
                canToggle = false;
            }
            else {
                Timing.RunCoroutine(ToggleDelay());
            }
        }

        void Update(){
            if (_occupied && Input.GetKeyDown(KeyCode.E)){
                Toggle();
            }
        }

        public IEnumerator<float> ToggleDelay() {
            canToggle = false;
            yield return Timing.WaitForSeconds(toggleSafetyTime);
            canToggle = true;
        }

        void OnTriggerEnter(Collider c) {
            if (c.CompareTag("Player")) {
                materialSwitch.Toggle();
                _occupied = true;
            }
        }

        void OnTriggerExit(Collider c){
            if (c.CompareTag("Player")){
                materialSwitch.Toggle();
                _occupied = false;
            }
        }

        public override void Initiate() {
            //do nothing
        }
    }
}
