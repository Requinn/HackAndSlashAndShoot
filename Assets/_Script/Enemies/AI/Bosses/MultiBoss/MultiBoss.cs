using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using UnityEngine;

namespace JLProject{
    public class MultiBoss : AIEntity {
        public MultiBossController[] BossObjects;
        public Transform[] EnabledPositions;
        public GameObject bossUI;
        private int _currentPhase;
        private int _headsDowned;

        void OnEnable(){
            BossObjects[_currentPhase].gameObject.SetActive(true);
            BossObjects[_currentPhase].Initiate();
            BossObjects[_currentPhase].transform.DOMove(EnabledPositions[_currentPhase].position, 0.5f);
            foreach (var bossobj in BossObjects){
                bossobj.OnDeath += CheckPhase;
            }
        }

        /// <summary>
        /// check if it's time to move to the next phase
        /// </summary>
        /// <param name="hp"></param>
        private void CheckPhase(){
            _headsDowned++;
            if (_currentPhase == 0 && _headsDowned == 1){
                ChangePhase(1);
                _headsDowned = 0;
            }
            if (_currentPhase == 1 && _headsDowned == 2){
                foreach (var boss in BossObjects){
                    boss.CanRevive = false;
                }
                ChangePhase(2);
                _headsDowned = 0;
            }
            if(_currentPhase == 2 && _headsDowned == 3){
                foreach (var boss in BossObjects){
                    boss.gameObject.SetActive(false);
                    Timing.KillCoroutines(boss.attackHandle);
                    bossUI.SetActive(false);
                }
                OnDeath();
            }
        }

        /// <summary>
        /// handle a phase change
        /// </summary>
        /// <param name="i"></param>
        private void ChangePhase(int i){
            _currentPhase = i;
            BossObjects[_currentPhase].gameObject.SetActive(true);
            BossObjects[_currentPhase].transform.DOMove(EnabledPositions[_currentPhase].position, 1f);
            foreach (var boss in BossObjects){
                Timing.KillCoroutines(boss.attackHandle);
            }
            Timing.RunCoroutine(SyncrhonizeDelay(2f));
        }

        private IEnumerator<float> SyncrhonizeDelay(float f){
            yield return Timing.WaitForSeconds(f);
            foreach (var boss in BossObjects){
                if (boss.gameObject.activeInHierarchy){
                    boss.Initiate();
                }
            }
        }

        protected override void Movement(){
            throw new System.NotImplementedException();
        }

        protected override void Attack(){
            throw new System.NotImplementedException();
        }

        public override void ProjectileResponse(){
            throw new System.NotImplementedException();
        }
    }
}
