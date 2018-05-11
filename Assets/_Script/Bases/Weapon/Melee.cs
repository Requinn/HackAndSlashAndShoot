using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace JLProject{
    public class Melee : Weapon{
        public float attackDelay = 0.65f;
        public float comboDelay = 0.75f;
        public int swingCount = 3;

        [SerializeField] protected int _currentCombo = 0;
        protected float _timeSinceSwing = 0.0f;
        [SerializeField] protected float _comboDecay = 0.7f;

        public GameObject[] WaveComponent;
        public int[] damageValues;
        public int[] force;
        public int[] momentum;
        public Damage.Faction faction;
        private ImpactReceiver _parentImpactRcvr;

        public int CurrentCombo{
            get{ return _currentCombo; }

            set{ _currentCombo = value; }
        }

        void Start(){
            _owningObj = GetComponentInParent<Entity>();
            _parentImpactRcvr = GetComponentInParent<ImpactReceiver>();
            AttackDelay = attackDelay;
            CurMag = MaxMag = swingCount;
            ReloadSpeed = comboDelay;
            //temporary!!!!
            //TODO: Change this so things like, 3rd hits do more damage in a combo or something similar!!
            for (int i = 0; i < WaveComponent.Length; i++){
                WaveComponent[i].GetComponent<WaveCollision>().args = new Damage.DamageEventArgs(damageValues[i], transform.position, Damage.DamageType.Melee, faction, force[i]);
            }
        }

        void Update(){
            if (_currentCombo > 0){
                _timeSinceSwing += Time.deltaTime;
            }
            if (_timeSinceSwing >= _comboDecay){
                _currentCombo = 0;
                _timeSinceSwing = 0.0f;
            }
        }

        public override void Fire(){
            if (_canAttack){
                _owningObj.AdjustSpeed(0f);
                if (_currentCombo > 0 && _parentImpactRcvr){
                    PushForward(_currentCombo);
                }
                _timeSinceSwing = 0.0f;
                //Debug.Log(_currentCombo);
                WaveComponent[_currentCombo].SetActive(true);
                WaveComponent[_currentCombo].GetComponent<MeshRenderer>().enabled =
                    true; //This is to re enable the mesh, for some reason it turns off and stays off
                Timing.RunCoroutine(WaveDelay(_currentCombo));
                _currentCombo++;
                if (_currentCombo == swingCount){
                    Timing.RunCoroutine(Reload());
                }
                else{
                    Timing.RunCoroutine(Delay());
                }
            }
        }

        public override void ChargeAttack(){
        }

        private void PushForward(int combo){
            _parentImpactRcvr.AddImpact(_parentImpactRcvr.transform.forward.normalized, momentum[combo]);
        }

        public override IEnumerator<float> Delay() {
            _canBlock = _canAttack = false;
            yield return Timing.WaitForSeconds(AttackDelay);
            _canBlock = _canAttack = true;
            _owningObj.ResetSpeed(); //attack is over
        }

        /// <summary>
        /// Temporary new? look into later
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> Reload(){
            _canBlock = _canAttack = false;
            yield return Timing.WaitForSeconds(attackDelay);
            _owningObj.ResetSpeed(); //attack is over
            _canBlock = true;
            yield return Timing.WaitForSeconds(comboDelay - attackDelay);
            _canAttack = true;
            _currentCombo = 0;
        }

        private IEnumerator<float> WaveDelay(int hitboxNo){
            yield return Timing.WaitForSeconds(0.025f);
            if (this) { 
                WaveComponent[hitboxNo].SetActive(false);
            }
        }
    }
}
