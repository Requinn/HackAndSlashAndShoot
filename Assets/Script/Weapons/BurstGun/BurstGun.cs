﻿using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject;
using MEC;
using UnityEngine;

public class BurstGun : Gun {
    public float ShotDelay = 0.15f;
    public float ReloadTime = 0.25f;
    public int burstCount = 5;
    public float burstDelay = 0.05f;
    public float spray = 0.1f;
    private AudioSource _gunSounds;
    public AudioClip[] gunAudio;
    public int MaxAmmo = 3;
    public Transform BarrelPoint;
    public Damage.Faction faction;
    private Damage.DamageEventArgs args;

    public delegate void BurstFiredEvent();

    private CoroutineHandle _delayHandle;
    public event BurstFiredEvent BurstStart, BurstEnd;
    // Use this for initialization
    void Start(){
        AttackDelay = ShotDelay;
        ReloadSpeed = ReloadTime;
        CurMag = MaxMag = MaxAmmo;
        ObjectPooler.ObjectPool.PoolItem(bullet);
        _gunSounds = GetComponent<AudioSource>();
    }

    private IEnumerator<float> Burst(){
        for (int i = 0; i < burstCount;){
            ReactivateBulletObj();
            CurMag--;
            if (CurMag == 0){
                Timing.KillCoroutines(_delayHandle);
                Timing.RunCoroutine(Reload());
                _gunSounds.PlayOneShot(gunAudio[1]);
                i = burstCount + 1;
            }
            else{
                yield return Timing.WaitForSeconds(burstDelay);
                i++;
            }
        }
        
    }

    private void ReactivateBulletObj(){
        GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
        if (bullet != null) {
            bullet.GetComponent<IProjectile>().SetFaction(faction);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            float deviation = Random.Range(-spray/2, spray/2); //a spray of bullets
            rb.velocity = transform.TransformDirection(new Vector3(deviation, 0, bullet.GetComponent<IProjectile>().GetVelocity()));
            bullet.transform.position = BarrelPoint.position;
            bullet.transform.rotation = GetComponentInParent<Transform>().rotation;
            _gunSounds.PlayOneShot(gunAudio[0]);
            bullet.SetActive(true);
        }
    }

    public override void Fire(){
        //get bullet from the object pool
        //pool objects on level start
        //don't allow weapon swapping mid mission to avoid repooling mid level
        if (_canAttack){
            Timing.RunCoroutine(Burst());
            _delayHandle = Timing.RunCoroutine(Delay());
        }
    }
}
