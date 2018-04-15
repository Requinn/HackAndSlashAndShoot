using UnityEngine;
using MEC;
using System.Collections.Generic;

namespace JLProject {
    /// <summary>
    /// simepl switch to open and close a door
    /// </summary>
    public class Switch : MonoBehaviour {
        public Toggleable[] Toggleables;
        public bool oneWay = false;
        public bool on = false;
        public bool canToggle = true;
        public float toggleSafetyTime = 0.5f;//can only toggle once per second

        public void Toggle(){
            if (canToggle){
                foreach (var T in Toggleables){
                    T.Toggle();
                }
                on = !on;
                Timing.RunCoroutine(ToggleDelay());
            }
        }

        public IEnumerator<float> ToggleDelay() {
            canToggle = false;
            yield return Timing.WaitForSeconds(toggleSafetyTime);
            canToggle = true;
        }
    }
}