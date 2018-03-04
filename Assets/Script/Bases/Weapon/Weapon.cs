using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace JLProject{
    public abstract class Weapon : MonoBehaviour{
        public int AttackValue{ get; set; }
        public float AttackDelay{ get; set; }
        public float ReloadSpeed{ get; set; }
        public int CurMag{ get; set; }
        public int MaxMag{ get; set; }
        public Damage.Faction Faction{ get; set; }
        protected bool _canAttack = true;
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
        public IEnumerator<float> Delay(){
            _canAttack = false;
            yield return Timing.WaitForSeconds(AttackDelay);
            _canAttack = true;
        }

        /// <summary>
        /// reloads the weapon's magazine/attack count
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Reload(){
            _canAttack = false;
            yield return Timing.WaitForSeconds(ReloadSpeed);
            CurMag = MaxMag;
            _canAttack = true;
        }
    }
}
