using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public class HealthPickup : MonoBehaviour{
        public int HealthRestored = 25;

        void OnTriggerEnter(Collider c){
            if (c.gameObject.CompareTag("Player")){
                c.GetComponent<Entity>().Heal(gameObject, HealthRestored);
                Destroy(gameObject);
            }
        }
    }
}