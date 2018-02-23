using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

public class ShortPistol : Gun{
    public float ShotDelay = 0.15f;
    public float ReloadTime = 0.25f;
    public int MaxAmmo = 3;
    public Transform BarrelPoint;
    private Damage.DamageEventArgs args;
	// Use this for initialization
	void Start (){
	    AttackDelay = ShotDelay;
	    ReloadSpeed = ReloadTime;
	    CurMag = MaxMag = MaxAmmo;
	}
	
    public override void Fire(){
        //get bullet from the object pool
        //pool objects on level start
        //don't allow weapon swapping mid mission to avoid repooling mid level
        if (_canAttack){
            GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
            if (bullet != null){
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity = transform.TransformDirection(new Vector3(bullet.GetComponent<IProjectile>().GetVelocity(), 0, 0));
                bullet.transform.position = BarrelPoint.position;
                bullet.transform.rotation = GetComponentInParent<Transform>().rotation;
                bullet.SetActive(true);
            }
            CurMag--;
            if (CurMag == 0){
                Timing.RunCoroutine(base.Reload());
            }
            else{
                Timing.RunCoroutine(base.Delay());
            }
        }
    }
}
