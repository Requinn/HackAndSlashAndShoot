﻿using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

public class LaserBeam : Weapon {
    [SerializeField]
    private List<Entity> _targets = new List<Entity>();
    public float Damage = 10.0f;
    public float Force = 15.0f;
    public float CastTime = 2.25f;
    public BoxCollider Collider;
    public AoEMarker MarkerScript;
    public Damage.DamageType dType = JLProject.Damage.DamageType.Explosive;
    public Damage.Faction Faction = JLProject.Damage.Faction.Enemy;
    private Damage.DamageEventArgs args;

    // Use this for initialization
    void Start (){
        Collider = GetComponent<BoxCollider>();
        args = new Damage.DamageEventArgs(Damage, this.transform.position, dType, Faction);
    }

    public override void Fire(){
        MarkerScript.StartCast(CastTime, 1);
        Timing.RunCoroutine(Explode());
    }

    private IEnumerator<float> Explode(){
        _canAttack = false;
        yield return Timing.WaitForSeconds(CastTime);
        if (this){
            foreach (Entity e in _targets){
                if (e.gameObject != null){
                    e.TakeDamage(this.gameObject, ref args);
                    var impact = e.GetComponent<ImpactReceiver>();
                    if (impact){
                        impact.AddImpact(
                            (e.transform.position - GetComponentInParent<Entity>().transform.position).normalized,
                            Force);
                    }
                }
                else{
                    _targets.Remove(e);
                }
            }
        }
        _canAttack = true;
    }

    //is this efficient??
    //Explosion using an always enabled trigger volume to amass potential targets, then executes the explosion on targets when told to do so
    void OnTriggerEnter(Collider c) {
        if (c.GetComponent<Entity>()) {
            Entity ent = c.gameObject.GetComponent<Entity>();
            if (ent.Faction == JLProject.Damage.Faction.Player) {
                _targets.Add(ent);
            }
        }
    }

    void OnTriggerExit(Collider c) {
        Entity ent = c.gameObject.GetComponent<Entity>();
        if (_targets.Contains(ent)) {
            _targets.Remove(ent);
        }
    }
}