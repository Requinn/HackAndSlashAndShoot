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
    public void OnEnable(){
        _curPos = transform.position;
        _lifetime = CalculateTimeToLive();
        Timing.RunCoroutine(DelayedRepool(_lifetime));
    }

    private float CalculateTimeToLive(){
        return FlightDistance / Velocity;
    }

    public void Update(){
        
    }

    public void OnTriggerEnter(Collider c){
        Entity ent = c.gameObject.GetComponent<Entity>();
        if (ent != null){
            if (ent.Faction != Damage.Faction.Player || ent.Faction != Damage.Faction.Allied){
                //fix this later
                args.HitPoint = transform.position;
                ent.TakeDamage(gameObject, ref args);
                if (statusObj){
                    ent.ApplyStatus(statusObj);
                }
                gameObject.SetActive(false);
            }
            StopCoroutine(DelayedRepool(_lifetime));
        }
        else if(c.gameObject.tag == "Environment"){
            gameObject.SetActive(false);
        }
    }

    private IEnumerator<float> DelayedRepool(float t){
        yield return Timing.WaitForSeconds(t);
        gameObject.SetActive(false);
    }

    public float GetVelocity(){
        return Velocity;
    }
}
