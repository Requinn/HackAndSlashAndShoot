using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public class CollisionReport : MonoBehaviour{
        public List<Entity> HitTargetList{ get; set; }

        void OnTriggerEnter(Collider c){
            if (c.gameObject.tag == "Enemy"){
                HitTargetList.Add(c.GetComponent<Entity>());
            }
        }
    }
}
