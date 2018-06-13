using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JLProject;
using MEC;
using UnityEngine;

public class HealingWeapon : Weapon{
    public List<Entity> targets = new List<Entity>();
    public int Damage = 10;
    public float CastTime = 1.5f;
    public float Radius = 4.0f;
    public SphereCollider Collider;
    public AoEMarker MarkerScript;
    public Damage.DamageType dmgType = JLProject.Damage.DamageType.Explosive;
    public Damage.Faction Faction = JLProject.Damage.Faction.Enemy;

    //change this to use a sphere cast or something???
    void Start(){
        Collider = GetComponent<SphereCollider>();
        Collider.radius = Radius;
    }

    public override void Fire(){
        MarkerScript.StartCast(CastTime, Radius);
        Timing.RunCoroutine(Explode());
    }

    private IEnumerator<float> Explode(){
        yield return Timing.WaitForSeconds(CastTime);

        Collider[] targets = Physics.OverlapSphere(transform.position, Radius);
        foreach (var t in targets){
            Entity e = t.GetComponent<Entity>();
            if (e && e.Faction == JLProject.Damage.Faction.Enemy){
                e.Heal(gameObject, Damage);
            }
        }

        /*
        foreach (Entity e in targets.ToList()){
            if (gameObject != null){
                if (e != null){
                    e.Heal(gameObject, Damage);
                }
                else{
                    targets.Remove(e);
                }
            }
        }
        */
    }

    //Currently used by the medic ai to have a list of low health enemies
    //fix it??
    void OnTriggerEnter(Collider c){
        if (c.GetComponent<Entity>()){
            Entity ent = c.gameObject.GetComponent<Entity>();
            if (ent.Faction == JLProject.Damage.Faction.Enemy){
                targets.Add(ent);
            }
        }
    }

    void OnTriggerExit(Collider c){
        Entity ent = c.gameObject.GetComponent<Entity>();
        if (targets.Contains(ent)){
            targets.Remove(ent);
        }
    }
}