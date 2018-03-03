using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

/// <summary>
/// shield base class
/// </summary>
namespace JLProject{
    public abstract class Shield : MonoBehaviour{
        [SerializeField]
        protected float _maximumhealth = 100.0f;
        //shield max health    
        public float MaxHealth {
            get { return _maximumhealth; }
            protected set { _maximumhealth = value; }
        }
        [SerializeField]
        protected float _currenthealth;
        //shield current health
        public float CurrentHealth {
            get { return _currenthealth; }
            protected set { _currenthealth = value; }
        }
        [SerializeField]
        protected float _rechargevalue;
        //how much shield we regain per tick
        public float RechargeValue {
            get { return _rechargevalue; }
            set { _rechargevalue = value; }
        }
        [SerializeField]
        protected float _rechargedelay;
        //how long we wait for recharge
        public float RechargeDelay {
            get { return _rechargedelay; }
            set { _rechargedelay = value; }
        }
        [SerializeField]
        protected float _rechargespeed;
        //how fast our ticks are
        public float RechargeSpeed {
            get { return _rechargespeed; }
            set { _rechargespeed = value; }
        }
        [SerializeField]
        protected float _brokenrechargedelay;
        //how long we wait on a broken shield
        public float BrokenRechargeDelay {
            get { return _brokenrechargedelay; }
            set { _brokenrechargedelay = value; }
        }

        public bool broken = false;
        public bool blocking = false;
        public HealthBar UIshield;

        public GameObject shieldBubble;
        private CoroutineHandle _rechargeHandle, _regenHandle, _brokenrechargeHandle;
        // Use this for initialization
        void Start(){
            _currenthealth = _maximumhealth;
            UpdateHealthUI();
        }

        void LateUpdate(){
            if (Input.GetMouseButtonDown(1)){
                ShieldBubbleOn();
            }else if (Input.GetMouseButtonUp(1)){
                ShieldBubbleOff();
            }
        }

        public void ShieldBubbleOn(){
            if (shieldBubble != null && !broken){
                shieldBubble.SetActive(true);
            }
        }
        public void ShieldBubbleOff() {
            if (shieldBubble != null) {
                shieldBubble.SetActive(false);
            }
        }

        public float HealthPercent() {
            return _currenthealth / _maximumhealth;
        }

        public void UpdateHealthUI() {
            if (UIshield) {
                UIshield.UpdateHealthBar(HealthPercent());
            }
        }

        /// <summary>
        /// blocks damage and starts regeneration
        /// </summary>
        /// <param name="damage"></param>
        public virtual void Block(float damage){
            Timing.KillCoroutines(_regenHandle);
            Timing.KillCoroutines(_rechargeHandle);
            Timing.KillCoroutines(_brokenrechargeHandle);

            _currenthealth = Mathf.Clamp(_currenthealth - damage,0,_maximumhealth);
            UpdateHealthUI();

            if (_currenthealth <= 0.0f){
                broken = true;
                ShieldBubbleOff();
                _brokenrechargeHandle = Timing.RunCoroutine(BrokenRecharge());
            }
            else{
                _rechargeHandle = Timing.RunCoroutine(RegularRecharge());
            }
            
        }

        //shield is broken, wait longer
        public virtual IEnumerator<float> BrokenRecharge(){
            yield return Timing.WaitForSeconds(_brokenrechargedelay);
            _regenHandle = Timing.RunCoroutine(Recharge());
        }

        //shield is recharging normally
        public virtual IEnumerator<float> RegularRecharge(){
            yield return Timing.WaitForSeconds(_rechargedelay);
            _regenHandle = Timing.RunCoroutine(Recharge());
        }

        //regain shield health
        private IEnumerator<float> Recharge(){
            while (_currenthealth < _maximumhealth) {
                _currenthealth += _rechargevalue;
                UpdateHealthUI();
                yield return Timing.WaitForSeconds(_rechargespeed);
            }
            broken = false;
        }
    }
}
