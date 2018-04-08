using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

public class ShortPistol : Gun{
    public float ShotDelay = 0.15f;
    public float ReloadTime = 0.25f;
    private AudioSource _gunSounds;
    public AudioClip[] gunAudio;
    public int MaxAmmo = 3;
    public Transform BarrelPoint;
    public Damage.Faction faction;
    private Damage.DamageEventArgs args;
	// Use this for initialization
	void Start (){
	    AttackDelay = ShotDelay;
	    ReloadSpeed = ReloadTime;
	    CurMag = MaxMag = MaxAmmo;
	    if (faction == Damage.Faction.Enemy)
	        bullet.objectToPool.tag = "EnemyProjectile";
	    ObjectPooler.ObjectPool.PoolItem(bullet);
	    _gunSounds = GetComponent<AudioSource>();
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
                rb.velocity = transform.TransformDirection(new Vector3(0, 0, bullet.GetComponent<IProjectile>().GetVelocity()));
                bullet.transform.position = BarrelPoint.position;
                bullet.transform.rotation = GetComponentInParent<Transform>().rotation;
                _gunSounds.PlayOneShot(gunAudio[0]);
                bullet.SetActive(true);
            }
            CurMag--;
            if (CurMag == 0){
                Timing.RunCoroutine(base.Reload());
                _gunSounds.PlayOneShot(gunAudio[1]);
            }
            else{
                Timing.RunCoroutine(base.Delay());
            }
        }
    }
}
