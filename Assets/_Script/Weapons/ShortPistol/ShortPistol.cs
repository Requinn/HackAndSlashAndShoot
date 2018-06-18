using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JLProject;
using JLProject.Weapons;
using MEC;
using UnityEngine;

public class ShortPistol : Gun{
    private AudioSource _gunSounds;
    public AudioClip[] gunAudio;
    public Transform BarrelPoint;
    private Damage.DamageEventArgs args;
    private WeaponsModHandler _modHandler;

    private float chargeTime = 3f;

    // Use this for initialization
    void Start(){
        //TODO: Weapon mod code
        //_modHandler = GetComponent<WeaponsModHandler>();
        //_modHandler.weapon = this;

        _gunSounds = GetComponent<AudioSource>();
        if (faction == Damage.Faction.Enemy)
            bullet.objectToPool.tag = "EnemyProjectile";
        if (ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool) == null){
            ObjectPooler.ObjectPool.PoolItem(bullet);
        }
    }

    public override void Fire(){
        //get bullet from the object pool
        //pool objects on level start
        //don't allow weapon swapping mid mission to avoid repooling mid level
        if (_canAttack){
            GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
            if (bullet != null){
                bullet.GetComponent<IProjectile>().SetFaction(faction);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity =
                    transform.TransformDirection(
                        new Vector3(0, 0, bullet.GetComponent<IProjectile>().GetVelocity()));
                bullet.transform.position = BarrelPoint.position;
                bullet.transform.rotation = GetComponentInParent<Transform>().rotation;
                _gunSounds.PlayOneShot(gunAudio[0]);
                bullet.GetComponent<ShortPistolBullet>().args.DamageValue = AttackValue;
                bullet.SetActive(true);
                CurMag--;
            }
            if (CurMag == 0){
                Timing.RunCoroutine(base.Reload());
                _gunSounds.PlayOneShot(gunAudio[1]);
            }
            else{
                Timing.RunCoroutine(base.Delay());
            }
        }
    }

    public override void ChargeAttack(){
        GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
        if (bullet != null){
            bullet.GetComponent<IProjectile>().SetFaction(faction);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity =
                transform.TransformDirection(
                    new Vector3(0, 0, bullet.GetComponent<IProjectile>().GetVelocity()));
            bullet.transform.position = BarrelPoint.position;
            bullet.transform.rotation = GetComponentInParent<Transform>().rotation;
            _gunSounds.PlayOneShot(gunAudio[0]);
            bullet.GetComponent<ShortPistolBullet>().args.DamageValue = AttackValue * 2.25f;
            bullet.SetActive(true);
        }
        Timing.RunCoroutine(base.Delay());
    }
}
