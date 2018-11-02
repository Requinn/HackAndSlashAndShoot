using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JLProject;
using JLProject.Weapons;
using MEC;
using UnityEngine;

public class Explosive : Weapon{
    [Header("Explosive Attributes")]
    public GameObject effect;
    [SerializeField]
    private GameObject _postExplosionObject;
    //TODO: All this stuff has to be cleaned up, we are inheriting from Weapon and much of this is kind of already there?
    public float Damage = 10.0f;
    public float Force = 25.0f;
    public float CastTime = 1.5f;
    public bool Repeated = false; //used for inplace explosions that are "static" on a level
    public float RepeatTime = 2f;
    public bool OneUse = false;
    public float Radius = 4.0f;
    public SphereCollider Collider;
    public AoEMarker MarkerScript;
    public Damage.DamageType dmgType = JLProject.Damage.DamageType.Explosive;
    public Damage.Faction damageFaction = JLProject.Damage.Faction.Enemy;
    private Damage.DamageEventArgs args;
    private EffectSettings _effectSettings;
    //change this to use a sphere cast or something???

    private CoroutineHandle _explosionCountdown;
    void Start(){
        Faction = damageFaction;
        Collider = GetComponent<SphereCollider>();
        Collider.radius = Radius;
        args = new Damage.DamageEventArgs(Damage, this.transform.position, dmgType, Faction);
        //set up particles
        if (effect){
            _effectSettings = effect.GetComponent<EffectSettings>();
            _effectSettings.DeactivateTimeDelay = RepeatTime;
            _effectSettings.MoveSpeed = (effect.transform.localPosition.y / CastTime) - 0.01f;
            _effectSettings.InstanceBehaviour = EffectSettings.DeactivationEnum.Deactivate;
            _effectSettings.MoveDistance = effect.transform.localPosition.y;
            _effectSettings.LayerMask = LayerMask.GetMask("Floor");

        }
        if (Repeated){
            InvokeRepeating("Fire", CastTime + RepeatTime, CastTime + RepeatTime);
        }

    }

    public override void Fire(){
        MarkerScript.StartCast(CastTime, Radius);
        //TODO: This effect shit needs to change
        if (effect){
            effect.transform.localPosition = new Vector3(0f, 10f, 0f);
            effect.SetActive(true);
        }
        _explosionCountdown = Timing.RunCoroutine(Explode());
        
    }

    /// <summary>
    /// STOP EXPLODING (COWARDS)
    /// Also destroys this bomb
    /// </summary>
    public void Extinguish(){
        Timing.KillCoroutines(_explosionCountdown);
        Destroy(gameObject);
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
                    Vector3 explosionNormal = (e.transform.position - transform.position).normalized;
                    explosionNormal.y = 0;
                    impact.AddImpact(explosionNormal, Force);
                }
            }
        }
        //If we have something to spawn after
        if (_postExplosionObject) {
            Instantiate(_postExplosionObject, transform.position, Quaternion.identity);
        }
        if (OneUse){
            Destroy(gameObject, 3f);
        }
    }
}
