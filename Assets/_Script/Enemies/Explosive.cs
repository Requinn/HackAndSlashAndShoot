using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JLProject;
using JLProject.Weapons;
using MEC;
using UnityEngine;

public class Explosive : Weapon{
    [Header("Explosive Attributes")]
    public float Damage = 10.0f;
    public float Force = 25.0f;
    public float CastTime = 1.5f;
    public bool Repeated = false; //used for inplace explosions that are "static" on a level
    public bool OneUse = false;
    public float Radius = 4.0f;
    public SphereCollider Collider;
    public AoEMarker MarkerScript;
    public Damage.DamageType dmgType = JLProject.Damage.DamageType.Explosive;
    public Damage.Faction damageFaction = JLProject.Damage.Faction.Enemy;
    private Damage.DamageEventArgs args;

    //change this to use a sphere cast or something???
    void Start(){
        Faction = damageFaction;
        Collider = GetComponent<SphereCollider>();
        Collider.radius = Radius;
        args = new Damage.DamageEventArgs(Damage, this.transform.position, dmgType, Faction);
        if (Repeated){
            InvokeRepeating("Fire", CastTime + 2.0f, CastTime + 2.0f);
        }

    }

    public override void Fire(){
        MarkerScript.StartCast(CastTime, Radius);
        Timing.RunCoroutine(Explode());
        
    }

    //replace list addition and removal with a physics.overlap pshere
    //save some tiem on read write and have a more controllable link to the aoe markers
    private IEnumerator<float> Explode(){
        yield return Timing.WaitForSeconds(CastTime);

        Collider[] targets = Physics.OverlapSphere(transform.position, Radius);
        foreach (var t in targets){
            Entity e = t.GetComponent<Entity>();
            if (e){
                e.TakeDamage(this.gameObject, ref args);
                ImpactReceiver impact = e.GetComponent<ImpactReceiver>();
                if (impact){
                    impact.AddImpact((e.transform.position - transform.position).normalized, Force);
                }
            }
        }
        if (OneUse){
            Destroy(gameObject);
        }
    }
}
