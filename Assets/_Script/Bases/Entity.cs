using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// Defines an entity object that is able to move, take/heal damage, and die
    /// </summary>
    public abstract class Entity : HealthSystem {
        public enum State{
            Attacking,
            Moving,
            Idle

        }
        [SerializeField]
        private float _movementspeed;

        public float MovementSpeed {
            get { return _movementspeed; }
            set { _movementspeed = _speedVar = value; }
        }

        private float _speedVar;
        protected abstract void Movement();

        /// <summary>
        /// adjust movement speed to the value
        /// </summary>
        /// <param name="newSpeed"></param>
        public void AdjustSpeed(float newSpeed){
            _movementspeed = newSpeed;
        }

        /// <summary>
        /// reset movement speed to the base value
        /// </summary>
        public void ResetSpeed(){
            _movementspeed = _speedVar;
        }
        /// <summary>
        /// perform an attack with the weapon in the current slot
        /// </summary>
        protected abstract void Attack();


        /// <summary>
        /// redundant
        /// </summary>
        /// <returns></returns>
        public Damage.Faction GetFaction(){
            return Faction;
        }

        public void ApplyDamage(object sender, ref Damage.DamageEventArgs args){
            throw new System.NotImplementedException();
        }

        
    }
}
