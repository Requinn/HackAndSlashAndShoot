using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace JLProject{
    public class TestDummy : Entity{
        public float speed;

        public EventItem eventItem;
        private bool _canActivate = true;

        // Use this for initialization
        void Start(){
            TookDamage += FireExplosion;
            AdjustSpeed(speed);
            Faction = Damage.Faction.Enemy;
        }

        void Update(){
            if (Input.GetKeyDown(KeyCode.T) && eventItem){
                eventItem.Activate();
            }
        }

        private void FireExplosion(Damage.DamageEventArgs args){
            if (_canActivate && eventItem) {
                eventItem.Activate();
                Timing.RunCoroutine(Delay());
            }
        }

        private IEnumerator<float> Delay(){
            _canActivate = false;
            yield return Timing.WaitForSeconds(2.0f);
            _canActivate = true;
        }

        protected override void Movement() {
        }


        protected override void Attack() {
        }
    }
}
