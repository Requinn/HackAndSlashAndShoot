using System.Collections;
using System.Collections.Generic;
using JLProject.Weapons;
using MEC;
using UnityEngine;

namespace JLProject{
    public class Boss1Beam : Weapon{
        public LaserBeam[] Beams;
        public float delay;
        public override void Fire(){
            if (delay > 0f){
                Timing.RunCoroutine(DelayedAttack());
            }
            else{
                foreach (var b in Beams){
                    if (b.hasTarget){
                        b.Fire();
                        break;
                    }
                }
            }
        }

        private IEnumerator<float> DelayedAttack() {
            yield return Timing.WaitForSeconds(delay);
            foreach (var b in Beams) {
                if (b.hasTarget) {
                    b.Fire();
                    break;
                }
            }
        }
    }
}
