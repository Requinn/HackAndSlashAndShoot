using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace JLProject.Weapons{
    public class Melee : Weapon{
        [SerializeField]
        protected float _timeSinceSwing = 0.0f;
        [Header("Melee Combo Attributes")]
        [SerializeField] protected float _comboDecay = 0.7f;
        public GameObject[] WaveComponent;
        public int[] damageValues;
        public int[] force;
        public int[] momentum;
        public Damage.Faction faction;
        protected ImpactReceiver _parentImpactRcvr;

        public int CurrentCombo{
            get{ return MaxMag - _currentMag; }
        }

        void Start(){
            _owningObj = GetComponentInParent<Entity>();
            _parentImpactRcvr = GetComponentInParent<ImpactReceiver>();
            //temporary!!!!
            //TODO: Change this so things like, 3rd hits do more damage in a combo or something similar!!
            for (int i = 0; i < WaveComponent.Length; i++){
                WaveComponent[i].GetComponent<WaveCollision>().args = new Damage.DamageEventArgs(damageValues[i], transform.position, Damage.DamageType.Melee, faction, force[i]);
            }
        }

        void Update(){
            if (_currentMag < MaxMag){
                _timeSinceSwing += Time.deltaTime;
            }
            if (_timeSinceSwing >= _comboDecay){
                _currentMag = MaxMag;
                _timeSinceSwing = 0.0f;
            }
        }

        public override void Fire(){
            if (_canAttack){
                _owningObj.AdjustSpeed(0f);
                if (_currentMag > 0 && _parentImpactRcvr){
                    PushForward(CurrentCombo);
                }
                _timeSinceSwing = 0.0f;
                //Debug.Log(_currentCombo);
                WaveComponent[CurrentCombo].SetActive(true);
                //This is to re enable the mesh, for some reason it turns off and stays off
                WaveComponent[CurrentCombo].GetComponent<MeshRenderer>().enabled =true; 
                Timing.RunCoroutine(WaveDelay(CurrentCombo));
                CurMag--;
                if (_currentMag == 0){
                    Timing.RunCoroutine(Reload());
                }
                else{
                    Timing.RunCoroutine(Delay());
                }
            }
        }

        public override void ChargeAttack(){
        }

        protected void PushForward(int combo){
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
            yield return Timing.WaitForSeconds(AttackDelay);
            _owningObj.ResetSpeed(); //attack is over
            _canBlock = true;
            yield return Timing.WaitForSeconds(ReloadSpeed - AttackDelay);
            _canAttack = true;
            _currentMag = MaxMag;
        }

        protected IEnumerator<float> WaveDelay(int hitboxNo){
            yield return Timing.WaitForSeconds(0.025f);
            if (this) { 
                WaveComponent[hitboxNo].SetActive(false);
            }
        }
    }
}
