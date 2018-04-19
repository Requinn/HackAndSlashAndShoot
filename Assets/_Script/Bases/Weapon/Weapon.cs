using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.UI;

namespace JLProject{
    public abstract class Weapon : MonoBehaviour{
        public int ReferenceID;
        public Sprite weaponIcon;
        public int AttackValue{ get; set; }
        public float AttackDelay{ get; set; }
        public float ReloadSpeed{ get; set; }
        public int CurMag{ get; set; }
        public int MaxMag{ get; set; }
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
