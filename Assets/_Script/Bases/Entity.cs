using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Defines an entity object that is able to move, take/heal damage, and die
/// </summary>
namespace JLProject{
    public abstract class Entity : HealthSystem {
        public enum State{
            Attacking,
            Moving,
            Idle

        }
        private float _movementspeed;

        public float MovementSpeed {
            get { return _movementspeed; }
            set { _movementspeed = value; }
        }

        protected abstract void Movement();

        /// <summary>
        /// perform an attack with the weapon in the current slot
        /// </summary>
        protected abstract void Attack();

        public Damage.Faction GetFaction(){
            throw new System.NotImplementedException();
        }

        public void ApplyDamage(object sender, ref Damage.DamageEventArgs args){
            throw new System.NotImplementedException();
        }

        
    }
}
