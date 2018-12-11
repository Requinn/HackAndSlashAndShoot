using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// Script for objects that can be picked up
    /// </summary>
    public class Pickuppable : MonoBehaviour{
        private BoxCollider _collider;
        public MaterialToggle toggle;
        public bool Selected = false;
        private Damage.DamageEventArgs args;

        void Start(){
            _collider = GetComponent<BoxCollider>();
        }

        void Update(){
            if (Selected && !toggle.Opened){
                toggle.Open();
            }
            else if(!Selected && toggle.Opened){
                toggle.Close();
            }
            Selected = false;
        }

        /// <summary>
        /// disable collider while object is held
        /// </summary>
        public void PickUp(){
            _collider.enabled = false;
        }

        /// <summary>
        /// re-enable the collider
        /// </summary>
        public void SetDamageOn(){
            _collider.enabled = true;
            _collider.isTrigger = true;
            Destroy(gameObject, 8.5f);
        }

        void OnTriggerEnter(Collider c){
            if (c.gameObject.CompareTag("Enemy")){
                args = new Damage.DamageEventArgs(5, transform.position, Damage.DamageType.Neutral, Damage.Faction.Player);
                c.GetComponent<Entity>().TakeDamage(this ,ref args);
                Destroy(gameObject);
            }
            BreakableObject brk = c.GetComponent<BreakableObject>();
            if (brk){
                brk.Break();
                Destroy(gameObject);
            }
            if (c.gameObject.CompareTag("Environment")) {
                Destroy(gameObject);
            }
            if (c.gameObject.CompareTag("Switch")){
                c.GetComponent<Switch>().Toggle();
                Destroy(gameObject);
            }
        }
    }
}
