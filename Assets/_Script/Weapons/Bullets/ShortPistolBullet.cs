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

    public void OnEnable(){
        _curPos = transform.position;
        _lifetime = CalculateTimeToLive();
        _repoolHandle = Timing.RunCoroutine(DelayedRepool(_lifetime));
    }

    private float CalculateTimeToLive(){
        return FlightDistance / Velocity;
    }

    public void SetFaction(Damage.Faction f){
        args.SourceFaction = f;
    }

    public void OnTriggerEnter(Collider c){
        Entity ent = c.GetComponent<Entity>();
        Switch swtch = c.GetComponent<Switch>();
        BreakableObject brk = c.GetComponent<BreakableObject>();
        if (ent != null){
            if (ent.Faction != Damage.Faction.Player || ent.Faction != Damage.Faction.Allied){
                //fix this later
                args.HitSourceLocation = transform.position;
                ent.TakeDamage(gameObject, ref args);
                if (statusObj){
                    ApplyStatus(statusObj, ent);
                }
                Timing.KillCoroutines(_repoolHandle);
                gameObject.SetActive(false);
            }
            StopCoroutine(DelayedRepool(_lifetime));
        }
        else if (c.gameObject.tag == "Environment"){
            Timing.KillCoroutines(_repoolHandle);
            gameObject.SetActive(false);
        }
        if (args.SourceFaction != Damage.Faction.Player) return;
        if (swtch){
            swtch.Toggle();
            Timing.KillCoroutines(_repoolHandle);
            gameObject.SetActive(false);
        }
        if (brk){
            brk.GetComponent<BreakableObject>().Hit();
            Timing.KillCoroutines(_repoolHandle);
            gameObject.SetActive(false);
        }

    }

    private IEnumerator<float> DelayedRepool(float t){
        yield return Timing.WaitForSeconds(t);
        Timing.KillCoroutines(_repoolHandle);
        gameObject.SetActive(false);
    }

    public float GetVelocity(){
        return Velocity;
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
}