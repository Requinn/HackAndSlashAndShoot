using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject {
    public class WaveCollision : MonoBehaviour{
        public Damage.DamageEventArgs args;
        public StatusObject statusObj;
        public Transform parentTransform;
        //TODO CLEAN UP THIS MESS

        void OnTriggerEnter(Collider c){
            //Debug.Log(c);
            Entity ent = c.GetComponent<Entity>();
            Switch swtch = c.GetComponent<Switch>();
            BreakableObject brk = c.GetComponent<BreakableObject>();
            if (ent != null){
                if (ent.Faction != args.SourceFaction || ent.Faction == Damage.Faction.Neutral){
                    //fix this later
                    args.HitSourceLocation = transform.position;
                    ent.TakeDamage(gameObject, ref args);
                    var Impact = ent.GetComponent<ImpactReceiver>();
                    if (Impact) {
                        Impact.AddImpact((ent.transform.position - parentTransform.position).normalized,args.HitForce);
                    }
                    if (statusObj){
                        ApplyStatus(statusObj, ent);
                    }
                }
            }
            if (swtch && args.SourceFaction == Damage.Faction.Player) {
                swtch.Toggle();
            }
            if (brk && args.SourceFaction == Damage.Faction.Player) {
                brk.GetComponent<BreakableObject>().Hit();
            }
            
        }
        /// <summary>
        /// Apply the status effect
        /// 
        /// FIX THIS TO STACK PROPERLY BY REFRESHING DURATION
        /// </summary>
        /// <param name="SO"></param>
        public void ApplyStatus(StatusObject SO, Entity E) {
            if (E.Afflictions.Contains(SO)) {
                E.Afflictions.Find(A => A.Type == SO.Type).InitializeProc();
            }
            else {
                E.Afflictions.Add(SO);
                Instantiate(SO, E.transform);
            }
        }
    }

}
