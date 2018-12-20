using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

namespace JLProject.Weapons{
    public enum WeaponType {
        ShortSword = 1, Pistol, Autogun, Rapier
    }

    public abstract class Weapon : MonoBehaviour{
        public int ReferenceID;
        public Sprite weaponIcon;
        [Header("Weapon Attributes")]
        [SerializeField] protected WeaponType _type;
        public WeaponType Category {
            get { return _type; }
        }

        [SerializeField] protected float _damage;
        public float AttackValue{
            get{ return _damage; }
            set{ _damage = value; }
        }
        [SerializeField] protected float _attackDelay;
        public float AttackDelay{
            get{ return _attackDelay; }
            set{ _attackDelay = value; }
        }
        [SerializeField] protected float _reloadSpeed;
        public float ReloadSpeed{
            get{ return _reloadSpeed; }
            set{ _reloadSpeed = value; }
        }
        [SerializeField] protected int _currentMag;
        public int CurMag{
            get{ return _currentMag; }
            set{ _currentMag = value; }
        }
        [SerializeField] protected int _maximumMag;
        public int MaxMag{
            get{ return _maximumMag; }
            set{ _maximumMag = value; }
        }
        /**
        [SerializeField] protected float _chargeTime;
        public float ChargeTime{
            get{ return _chargeTime; }
            set{ _chargeTime = value; }
        }
        [SerializeField] protected float _chargeAttackDamageBonus;
        public float ChargeDamageBonus{
            get{ return _chargeAttackDamageBonus; }
            set{ _chargeAttackDamageBonus = value; }
        }
        **/
        public Type damageType;
        public Damage.Faction Faction { get; set; }
        public enum Type {
            Melee,
            Ranged
        } 
        public float movementWeight; //how much is the player slowed down when firing this weapon (Automatics only)
        public bool isAutomatic = false;
        protected Entity _owningObj;
        //public List<string> StatsList = new List<string>(){"Damage", "AttackDelay", "ReloadSpeed", "MagazineSize"};

        public bool _canAttack = true;
        public bool _canBlock = true;


        /// <summary>
        /// perform the attack
        /// </summary>
        public abstract void Fire();

        public virtual void ChargeAttack(){
            //Nothing
        }
        /// <summary>
        /// the delay between attacks
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<float> Delay(){
            _canBlock = _canAttack = false;
            yield return Timing.WaitForSeconds(AttackDelay);
            _canBlock = _canAttack = true;
        }

        public delegate void ReloadEvent(float time);
        public ReloadEvent onReloadStart;
        /// <summary>
        /// reloads the weapon's magazine/attack count
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<float> Reload(){
            if (onReloadStart != null) onReloadStart(ReloadSpeed);
            _canAttack = false;
            yield return Timing.WaitForSeconds(ReloadSpeed);
            CurMag = MaxMag;
            _canBlock = _canAttack = true;
        }

        /// <summary>
        /// For Automatic weapons, reset their spools
        /// </summary>
        public virtual void ResetWeapon(){
            //Automatics only, for now???
        }
    }
}
