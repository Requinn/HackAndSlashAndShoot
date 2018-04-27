﻿using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

namespace JLProject{
    public abstract class Weapon : MonoBehaviour{
        public int ReferenceID;
        public Sprite weaponIcon;
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

        public List<string> StatsList = new List<string>(){"Damage", "AttackDelay", "ReloadSpeed", "MagazineSize"};
        public Damage.Faction Faction{ get; set; }
        public bool _canAttack = true;
        public bool _canBlock = true;
        public Type type;
        public enum Type{
            Melee,
            Ranged
        }

        /// <summary>
        /// perform the attack
        /// </summary>
        public abstract void Fire();

        /// <summary>
        /// the delay between attacks
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<float> Delay(){
            _canBlock = _canAttack = false;
            yield return Timing.WaitForSeconds(AttackDelay);
            _canBlock = _canAttack = true;
        }

        /// <summary>
        /// reloads the weapon's magazine/attack count
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<float> Reload(){
            _canAttack = false;
            yield return Timing.WaitForSeconds(ReloadSpeed);
            CurMag = MaxMag;
            _canBlock = _canAttack = true;
        }
    }
}
