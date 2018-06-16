using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace JLProject{
    public class SurvivalObjective : LevelObjective{
        public List<SpawnWave> waves = new List<SpawnWave>();
        public int currentWave = 0;

        public override void Initiate(){
            base.Initiate();

            waves[currentWave].Spawn();
            foreach (var w in waves){
                w.OnCompleteWave += StartNextWave;
            }
        }

        private void StartNextWave(){
            if (currentWave < waves.Count - 1){
                currentWave++;
                waves[currentWave].Spawn();
            }
            else{
                OpenDoors();
                isObjectiveComplete = true;
                OnCompleteObjective();
            }
        }

        void OnTriggerEnter(Collider c){
            if (c.gameObject.tag == "Player" && !isObjectiveComplete) {
                Initiate();
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}