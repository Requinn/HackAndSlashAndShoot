using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace JLProject{
    public class ShortWave : Melee{
        public GameObject[] WaveComponent;
        public int[] damageValues;
        public int[] momentum;
        public Damage.Faction faction;
        private ImpactReceiver _parentImpactRcvr;

        void Start(){
            _parentImpactRcvr = GetComponentInParent<ImpactReceiver>();
            AttackDelay = attackDelay;
            CurMag = MaxMag = swingCount;
            ReloadSpeed = comboDelay;
            //temporary!!!!
            //TODO: Change this so things like, 3rd hits do more damage in a combo or something similar!!
            for(int i = 0; i < WaveComponent.Length; i++) { 
                WaveComponent[i].GetComponent<WaveCollision>().args = new Damage.DamageEventArgs(damageValues[i],
                    this.transform.position, JLProject.Damage.DamageType.Melee, faction);
            }
        }

        void Update(){
            if (_currentCombo > 0) {
                _timeSinceSwing += Time.deltaTime;
            }
            if (_timeSinceSwing >= _comboDecay) {
                _currentCombo = 0;
                _timeSinceSwing = 0.0f;
            }
        }

        public override void Fire(){
            if (_canAttack){
                if (_currentCombo > 0 && _parentImpactRcvr){
                    PushForward(_currentCombo);
                }
                _timeSinceSwing = 0.0f;
                //Debug.Log(_currentCombo);
                WaveComponent[_currentCombo].SetActive(true);
                WaveComponent[_currentCombo].GetComponent<MeshRenderer>().enabled = true; //This is to re enable the mesh, for some reason it turns off and stays off
                Timing.RunCoroutine(WaveDelay(_currentCombo));
                _currentCombo++;
                if (_currentCombo == swingCount) {
                    Timing.RunCoroutine(Reload());
                }
                else{
                    Timing.RunCoroutine(Delay());
                }
            }
        }

        private void PushForward(int combo){
            _parentImpactRcvr.AddImpact(_parentImpactRcvr.transform.forward.normalized, momentum[combo]);
        }

        /// <summary>
        /// Temporary new? look into later
        /// </summary>
        /// <returns></returns>
        private new IEnumerator<float> Reload() {
            _canAttack = false;
            yield return Timing.WaitForSeconds(comboDelay);
            _canAttack = true;
            _currentCombo = 0;
        }

        private IEnumerator<float> WaveDelay(int hitboxNo){
            if (this){
                yield return Timing.WaitForSeconds(0.025f);
                WaveComponent[hitboxNo].SetActive(false);
            }
        }
    }

}

