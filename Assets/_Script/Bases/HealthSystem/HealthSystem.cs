using System;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// A base class to handle health values on all entities
    /// - Big Thanks to Michael Wolf for letting me use some of his ideas
    /// </summary>
    public abstract class HealthSystem : MonoBehaviour{
        [SerializeField] protected float _maximumhealth = 100.0f;
        public float MaxHealth{
            get{ return _maximumhealth; }
            protected set{ _maximumhealth = value; }
        }
        [SerializeField] protected float _currenthealth;
        public float CurrentHealth{
            get{ return _currenthealth; }
            protected set{ _currenthealth = value; }
        }
        [SerializeField] protected float _armorvalue;
        public float ArmorValue{
            get{ return _armorvalue; }
            set{ _armorvalue = value; }
        }
        [SerializeField] protected Damage.Faction _faction;
        public Damage.Faction Faction{
            get{ return _faction; }
            set{ _faction = value; }
        }
        
        public List<StatusObject.StatusType> StatusImmunities = new List<StatusObject.StatusType>(); 
        //WORK ON THIS 
        public List<StatusObject> Afflictions = new List<StatusObject>();

        public HealthBar UIhp;
        public Shield shield;
        public DamageFloaterSpawner floater;

        public bool IsDead{ get; protected set; }
        public bool CanRevive = false;
        public bool CanMove = true;

        protected Action onDeath = delegate{ };
        public Action OnDeath{
            get{ return onDeath; }
            set{ onDeath = value; }
        }

        protected Action onRevive = delegate{  };
        public Action OnRevive{
            get{ return onRevive; }
            set{ onRevive = value; }
        }

        public delegate void TookDamageEvent(Damage.DamageEventArgs args);
        public event TookDamageEvent TookDamage;
        public event TookDamageEvent TookShieldDamage;

        public delegate void TookStatusDamageEvent(float damage);
        public event TookStatusDamageEvent TookStatusDamage;

        public delegate void HealDamageEvent(float hp);
        public event HealDamageEvent HealDamage;

        // Use this for initialization
        void Start(){
            _currenthealth = _maximumhealth;
        }

        public float HealthPercent(){
            return _currenthealth / _maximumhealth;
        }

        public void UpdateHealthUI(){
            if (UIhp){
                UIhp.UpdateHealthBar(HealthPercent());
            }
        }
        /// <summary>
        /// lose some health = [damage - armor]
        /// </summary>
        /// <param name="damage"></param>
        public virtual void TakeDamage(object source, ref Damage.DamageEventArgs args){
            if (shield != null && shield.blocking){
                if(floater)floater.SpawnShieldText(args.DamageValue);
                shield.Block(args.DamageValue);
                if(TookShieldDamage != null) TookShieldDamage(args);
            }
            else{
                if (_maximumhealth > 0 && args.SourceFaction != _faction){
                    float calculatedDamage = CalculateDamage(args);
                    _currenthealth = Mathf.Clamp(_currenthealth - calculatedDamage, 0, _maximumhealth);
                    Damage.DamageEventArgs temp = args;
                    temp.DamageValue = calculatedDamage;
                    if (floater) floater.SpawnDamageText(calculatedDamage);
                    if (TookDamage != null) TookDamage(temp);
                    UpdateHealthUI();
                    if (_currenthealth == 0){
                        HandleDeath();
                    }
                }
            }
        }

        /// <summary>
        /// take damage unmitigated by armor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public virtual void TakeRawDamage(object source, ref Damage.DamageEventArgs args){
            if (shield != null && shield.blocking) {
                if (floater) floater.SpawnShieldText(args.DamageValue);
                shield.Block(args.DamageValue);
            }
            else {
                if (_maximumhealth > 0 && args.SourceFaction != _faction) {
                    _currenthealth = Mathf.Clamp(_currenthealth - args.DamageValue, 0, _maximumhealth);
                    Damage.DamageEventArgs temp = args;
                    temp.DamageValue = args.DamageValue;
                    if (floater) floater.SpawnDamageText(args.DamageValue);
                    if (TookDamage != null) TookDamage(temp);
                    UpdateHealthUI();
                    if (_currenthealth == 0) {
                        HandleDeath();
                    }
                }
            }
        }
        /// <summary>
        /// Calculates the effective damage on the HealthManager, AFTER mutators.
        /// Override this for changing how damage is handled specifically on this class.
        /// Alternatively, attach a mutator. -Michael W.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual float CalculateDamage(Damage.DamageEventArgs e) {
            // reduce damage by ARMORVALUE%
            return (int)Mathf.Clamp(e.DamageValue - (e.DamageValue * (_armorvalue * 0.01f)), 0, _maximumhealth);
        }

        /// <summary>
        /// take some damage from a status effect, this damage is unmitigated
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="type"></param>
        public virtual void TakeStatusDamage(float damage, StatusObject.StatusType type){
            if (!StatusImmunities.Contains(type)){
                if (_maximumhealth > 0){
                    switch (type){
                        case StatusObject.StatusType.Bleed:
                            _currenthealth = Mathf.Clamp(_currenthealth - damage, 0, _maximumhealth);
                            if (TookDamage != null) TookStatusDamage(damage);
                            if (floater) floater.SpawnDamageText(damage);
                            UpdateHealthUI();
                            break;
                        case StatusObject.StatusType.Shock: //WIP
                            Timing.RunCoroutine(DelayToggle(damage));
                            break;
                    }
                    
                    if (_currenthealth == 0){
                        HandleDeath();
                    }
                }
            }
            else{
                if (TookStatusDamage != null) TookStatusDamage(0);
            }
        }

        //TODO: REmove??
        private IEnumerator<float> DelayToggle(float f){
            CanMove = false;
            yield return Timing.WaitForSeconds(f);
            CanMove = true;
        }

        /// <summary>
        /// heal some health
        /// </summary>
        /// <param name="heal"></param>
        public virtual void Heal(object source, int heal){
            _currenthealth = Mathf.Clamp(_currenthealth + heal, 0, _maximumhealth);
            if (floater) floater.SpawnHealText(heal);
            if (HealDamage != null) HealDamage(heal);
            UpdateHealthUI();
        }

        protected virtual void HandleDeath(){
            if (IsDead) return;
            //CHANGE THIS CODE
            IsDead = true;
            OnDeath();
            Destroy(gameObject, 1.0f);
        }

        protected virtual void HandleRevive(){
            if (!IsDead) return;
            IsDead = false;
            CurrentHealth = MaxHealth;
            UpdateHealthUI();
            OnRevive();
        }
    }
}
