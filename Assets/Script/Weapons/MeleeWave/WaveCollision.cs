using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject {
    public class WaveCollision : MonoBehaviour{
        public Damage.DamageEventArgs args;
        public StatusObject statusObj;
        //TODO CLEAN UP THIS MESS

        void OnTriggerEnter(Collider c){
            Entity ent = c.GetComponent<Entity>();
            Switch swtch = c.GetComponent<Switch>();
            if (ent != null){
                if (ent.Faction != args.SourceFaction || ent.Faction == Damage.Faction.Neutral){
                    //fix this later
                    args.HitPoint = transform.position;
                    ent.TakeDamage(gameObject, ref args);
                    if (statusObj){
                        ent.ApplyStatus(statusObj);
                    }
                }
            }
            if (swtch){
                swtch.Toggle();
            }
        }
    }
}
