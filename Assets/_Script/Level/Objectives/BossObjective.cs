using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace JLProject{
    public class BossObjective : LevelObjective{
        public AIEntity BossEnemy;
        private ObjectiveType objective = ObjectiveType.Kill;
        public CinemachineVirtualCamera BossCamera;

        void Awake(){
            BossCamera.Priority = 0;
        }

        public override void Initiate(){
            base.Initiate();

            BossCamera.Priority = 20;
            BossEnemy.gameObject.SetActive(true);
            BossEnemy.OnDeath += BossKilled;
        }

        private void BossKilled(){
            OnCompleteObjective();
            OpenDoors();
            BossCamera.Priority = 0;
            isObjectiveComplete = true;
        }

        protected override void OpenDoors() {
            if (ObjectToUnlock) {
                ObjectToUnlock.Locked = false;
                ObjectToUnlock.Open();
            }
            if (!oneWay && ObjectToLock) {
                ObjectToLock.Locked = false;
                ObjectToLock.Open();
            }
        }

        void OnTriggerEnter(Collider c) {
            if (c.gameObject.tag == "Player" && !isObjectiveComplete) {
                Initiate();
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}