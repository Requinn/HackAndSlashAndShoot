using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JLProject;
using MEC;
using UnityEngine;

public class Explosive : Weapon{
    [SerializeField]
    private List<Entity> _targets = new List<Entity>();
    public float Damage = 10.0f;
    public float Force = 25.0f;
    public float CastTime = 1.5f;
    private float Radius = 4.0f;
    public SphereCollider Collider;
    public AoEMarker MarkerScript;
    public Damage.DamageType dmgType = JLProject.Damage.DamageType.Explosive;
    public Damage.Faction Faction = JLProject.Damage.Faction.Enemy;
    private Damage.DamageEventArgs args;

    //change this to use a sphere cast or something???
    void Start(){
        Collider = GetComponent<SphereCollider>();
        Radius = Collider.radius;
        args = new Damage.DamageEventArgs(Damage, this.transform.position, Type, Faction);

    }

    public override void Fire(){
        MarkerScript.StartCast(CastTime, Radius);
        Timing.RunCoroutine(Explode());
        
    }

    private IEnumerator<float> Explode(){
        yield return Timing.WaitForSeconds(CastTime);
        foreach (Entity e in _targets) {
            if (e.gameObject != null) {
                e.TakeDamage(this.gameObject, ref args);
                var impact = e.GetComponent<ImpactReceiver>();
                if (impact) {
                    impact.AddImpact((e.transform.position - transform.position).normalized, Force);
                }
            }
            else {
                _targets.Remove(e);
            }
        }
    }

    //is this efficient??
    //Explosion using an always enabled trigger volume to amass potential targets, then executes the explosion on targets when told to do so
    void OnTriggerEnter(Collider c){
        if (c.GetComponent<Entity>()){
            Entity ent = c.gameObject.GetComponent<Entity>();
            if (ent.Faction == JLProject.Damage.Faction.Player){
                _targets.Add(ent);
            }
        }
    }

    void OnTriggerExit(Collider c){
        Entity ent = c.gameObject.GetComponent<Entity>();
        if (_targets.Contains(ent)){
            _targets.Remove(ent);
        }
    }
}
