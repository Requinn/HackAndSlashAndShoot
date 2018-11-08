using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JLProject;
using MEC;
using UnityEngine;

public class ShortPistolBullet : MonoBehaviour, IProjectile{
    public Damage.DamageEventArgs args;
    public float FlightDistance = 7.5f;
    public float Velocity = 5.0f;
    private Vector3 _curPos;
    private float _lifetime;
    public StatusObject statusObj;
    private CoroutineHandle _repoolHandle;
    private RaycastHit hit;
    private Ray ray;

    public void OnEnable(){
        _curPos = transform.position;
        _lifetime = CalculateTimeToLive();
        _repoolHandle = Timing.RunCoroutine(DelayedRepool(_lifetime));

    }

    //Raycast ahead for high velocity bullets
    void FixedUpdate(){
        ray = new Ray(transform.position, (transform.position - _curPos).normalized);
        Physics.Raycast(ray, out hit, 1f);
        if (hit.transform){
            DoCollisionCheck(hit.transform.gameObject);
        }
    }

    /// <summary>
    /// how long we should live before being repooled
    /// </summary>
    /// <returns></returns>
    private float CalculateTimeToLive(){
        return FlightDistance / Velocity;
    }

    public void SetFaction(Damage.Faction f){
        args.SourceFaction = f;
    }

    /// <summary>
    /// Check for collision
    /// </summary>
    /// <param name="hitObject"></param>
    void DoCollisionCheck(GameObject hitObject){
        Entity ent = hitObject.GetComponent<Entity>();
        Switch swtch = hitObject.GetComponent<Switch>();
        BreakableObject brk = hitObject.GetComponent<BreakableObject>();
        //We did hit an entity
        if (ent != null) {
            //if we are the enemy, and what we hit was labeled an enemy, continue through them
            if(args.SourceFaction == Damage.Faction.Enemy && ent.Faction == Damage.Faction.Enemy) {
                return;
            }
            //if they aren't a player, or an ally
            if (ent.Faction != Damage.Faction.Player || ent.Faction != Damage.Faction.Allied) {
                //fix this later
                args.HitSourceLocation = transform.position;
                ent.TakeDamage(gameObject, ref args);
                if (statusObj) {
                    ApplyStatus(statusObj, ent);
                }
                Timing.KillCoroutines(_repoolHandle);
                gameObject.SetActive(false);
            }
            StopCoroutine(DelayedRepool(_lifetime));
        }
        else if (hitObject.gameObject.tag == "Environment") {
            Timing.KillCoroutines(_repoolHandle);
            gameObject.SetActive(false);
        }
        if (args.SourceFaction != Damage.Faction.Player) return;
        if (swtch) {
            swtch.Toggle();
            Timing.KillCoroutines(_repoolHandle);
            gameObject.SetActive(false);
        }
        if (brk) {
            brk.GetComponent<BreakableObject>().Hit();
            Timing.KillCoroutines(_repoolHandle);
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider c){
       DoCollisionCheck(c.gameObject);
    }

    /// <summary>
    /// repool bullet if its life is up
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private IEnumerator<float> DelayedRepool(float t){
        yield return Timing.WaitForSeconds(t);
        Timing.KillCoroutines(_repoolHandle);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Apply the status effect, refreshing if it exists
    /// </summary>
    /// <param name="SO"></param>
    public void ApplyStatus(StatusObject SO, Entity E){
        StatusObject temp = E.Afflictions.Find(a => a.Type == SO.Type);
        if (temp){
            temp.InitializeProc();
        }
        else{
            E.Afflictions.Add(Instantiate(SO, E.transform));
        }
    }

    public void ResetLife() {
        Timing.KillCoroutines(_repoolHandle);
        _lifetime = CalculateTimeToLive();
        _repoolHandle = Timing.RunCoroutine(DelayedRepool(_lifetime));
    }

    public float GetVelocity() {
        return Velocity;
    }

    public Damage.Faction GetFaction() {
       return args.SourceFaction;
    }
}