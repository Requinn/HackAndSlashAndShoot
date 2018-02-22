using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject {
    public class WaveCollision : MonoBehaviour{
        public Damage.DamageEventArgs args;

        void OnTriggerEnter(Collider c){
            if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "Neutral"){
                c.gameObject.GetComponent<Entity>().TakeDamage(this.gameObject, ref args);
            }
        }
    }
}
