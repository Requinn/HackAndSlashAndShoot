using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// A base class to handle health values on all entities
/// - Big Thanks to Michael Wolf for letting me use some of his ideas
/// </summary>
namespace JLProject{
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
        public bool CanRevive{ get; protected set; }

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

        public delegate void TookDamageEvent(float hp);
        public event TookDamageEvent TookDamage;

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
                floater.SpawnShieldText(args.DamageValue);
                shield.Block(args.DamageValue);
            }
            else{
                if (_maximumhealth > 0 && args.SourceFaction != _faction){
                    _currenthealth = Mathf.Clamp(_currenthealth - ((int) args.DamageValue - _armorvalue), 0, _maximumhealth);
                    floater.SpawnDamageText(args.DamageValue);
                    if (TookDamage != null) TookDamage(args.DamageValue);
                    UpdateHealthUI();
                    if (_currenthealth == 0){
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
            // For the simplest case, just pass the raw DamageValue.
            return e.DamageValue;
        }

        /// <summary>
        /// take some damage from a status effect, this damage is unmitigated
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="type"></param>
        public virtual void TakeStatusDamage(float damage, StatusObject.StatusType type){
            if (!StatusImmunities.Contains(type)){
                if (_maximumhealth > 0){
                    _currenthealth = Mathf.Clamp(_currenthealth - damage, 0, _maximumhealth);
                    if (TookDamage != null) TookDamage(damage);
                    UpdateHealthUI();
                    if (_currenthealth == 0){
                        HandleDeath();
                    }
                }
            }
            else{
                if (TookDamage != null) TookDamage(0);
            }
        }

        /// <summary>
        /// heal some health
        /// </summary>
        /// <param name="heal"></param>
        public virtual void Heal(object source, int heal){
            _currenthealth = Mathf.Clamp(_currenthealth + heal, 0, _maximumhealth);
            floater.SpawnHealText(heal);
            if (HealDamage != null) HealDamage(heal);
            UpdateHealthUI();
        }

        protected virtual void HandleDeath(){
            if (IsDead) return;
            //CHANGE THIS CODE
            Destroy(gameObject);
            IsDead = true;
            OnDeath();
        }

        protected virtual void HandleRevive(){
            if (!IsDead) return;
            IsDead = false;
            OnRevive();
        }
    }
}
