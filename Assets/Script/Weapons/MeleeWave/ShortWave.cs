using System.Collections.Generic;
using MEC;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace JLProject{
    public class ShortWave : Melee{
        public GameObject[] WaveComponent;
        public float attackDelay = 0.65f;
        public float comboDelay = 1.25f;
        public int swingCount = 3;
        
        private int _currentCombo = 0;
        private float _timeSinceSwing = 0.0f;
        public Damage.Faction faction;
        public float Damage = 15.0f;
        void Start(){
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
                _timeSinceSwing = 0.0f;
                Debug.Log(_currentCombo);
                WaveComponent[_currentCombo].SetActive(true);
                WaveComponent[_currentCombo].GetComponent<MeshRenderer>().enabled = true; //This is to re enable the mesh, for some reason it turns off and stays off
                Timing.RunCoroutine(WaveDelay(_currentCombo));
                _currentCombo++;
                if (_currentCombo == 3){
                    Timing.RunCoroutine(Reload());
                }
                else{
                    Timing.RunCoroutine(Delay());
                }
            }
        }

        private new IEnumerator<float> Reload() {
            _canAttack = false;
            yield return Timing.WaitForSeconds(ReloadSpeed);
            _canAttack = true;
            _currentCombo = 0;
        }

        private IEnumerator<float> WaveDelay(int hitboxNo){
            yield return Timing.WaitForSeconds(0.025f);
            WaveComponent[hitboxNo].SetActive(false);
        }
    }

}

