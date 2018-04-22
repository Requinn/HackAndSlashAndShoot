using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace JLProject{
    public class BossObjective : LevelObjective{
        public bool oneWay = true;
        private bool completed = false;
        public AIEntity BossEnemy;
        private ObjectiveType objective = ObjectiveType.Kill;
        public CinemachineVirtualCamera BossCamera;

        public override void Initiate(){
            BossCamera.Priority = 20;
            if (ObjectToLock) {
                ObjectToLock.Locked = true;
                ObjectToLock.Close();
            }

            BossEnemy.gameObject.SetActive(true);
            BossEnemy.OnDeath += BossKilled;
        }

        private void BossKilled(){
            OnCompleteObjective();
            OpenDoors();
            BossCamera.Priority = 0;
            completed = true;
        }

        private void OpenDoors() {
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
            if (c.gameObject.tag == "Player" && !completed) {
                Initiate();
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}