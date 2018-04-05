using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace JLProject{
    public class ShortWave : Melee{
        public GameObject[] WaveComponent;
        public float attackDelay = 0.65f;
        public float comboDelay = 0.75f;
        public int swingCount = 3;
        
        private int _currentCombo = 0;
        private float _timeSinceSwing = 0.0f;
        public Damage.Faction faction;
        public float Damage = 15.0f;
        private ImpactReceiver _parentImpactRcvr;

        void Start(){
            _parentImpactRcvr = GetComponentInParent<ImpactReceiver>();
            AttackDelay = attackDelay;
            CurMag = MaxMag = swingCount;
            ReloadSpeed = comboDelay;
            //temporary!!!!
            //TODO: Change this so things like, 3rd hits do more damage in a combo or something similar!!
            foreach (var meleehitbox in WaveComponent){
                meleehitbox.GetComponent<WaveCollision>().args = new Damage.DamageEventArgs(Damage,
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
                if (_currentCombo > 0){
                    PushForward();
                }
                _timeSinceSwing = 0.0f;
                Debug.Log(_currentCombo);
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

        private void PushForward(){
            _parentImpactRcvr.AddImpact(_parentImpactRcvr.transform.forward.normalized, 25.0f);
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
            yield return Timing.WaitForSeconds(0.025f);
            WaveComponent[hitboxNo].SetActive(false);
        }
    }

}

