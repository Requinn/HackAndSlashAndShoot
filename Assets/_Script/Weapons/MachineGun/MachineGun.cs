using System.Collections;
using System.Collections.Generic;
using JLProject;
using JLProject.Weapons;
using MEC;
using NUnit.Framework.Constraints;
using UnityEngine;

public class MachineGun : Gun{
    [SerializeField] private float accuracy;
    private float spinUpIncrement; //how much rof we gain per shot
    [SerializeField] private int shotsToMaxSpin; //how many shots til we are at our fastest
    [SerializeField] private float maxSpinDelay; //the fastest we can shoot at
    [SerializeField] private float minSpinDelay; //the slowest we can shoot at
    public float accuracyMin, accuracyMax; //our minimum and maximum accuracy
    private float accuracyStep; //how much of our accuracy changes per shot

    private AudioSource _gunSounds;
    public AudioClip[] gunAudio;
    public Transform BarrelPoint;
    
    
    // Use this for initialization
    void Start(){
        _owningObj = GetComponentInParent<Entity>();
        _gunSounds = GetComponent<AudioSource>();
        if (faction == Damage.Faction.Enemy)
            bullet.objectToPool.tag = "EnemyProjectile";
        ObjectPooler.ObjectPool.PoolItem(bullet);

        minSpinDelay = AttackDelay;
        spinUpIncrement = (minSpinDelay - maxSpinDelay) / shotsToMaxSpin; //calucluate how much we wait between each bullet fired before the next
        accuracyStep =  (accuracyMax - accuracyMin) / shotsToMaxSpin; //how much accuracy we lose too
    }

    void OnEnable(){
        ResetWeapon();
    }

    private void ReactivateBulletObj() {
        GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
        if (bullet != null) {
            bullet.GetComponent<IProjectile>().SetFaction(faction);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            float deviation = Random.Range(-accuracy / 2, accuracy / 2); //a spray of bullets
            rb.velocity =
                transform.TransformDirection(new Vector3(deviation, 0,
                    bullet.GetComponent<IProjectile>().GetVelocity()));
            bullet.GetComponent<ShortPistolBullet>().args.DamageValue = AttackValue;
            bullet.transform.position = BarrelPoint.position;
            bullet.transform.rotation = GetComponentInParent<Transform>().rotation;
            _gunSounds.PlayOneShot(gunAudio[0]);
            bullet.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update(){

    }

    public override void Fire() {
        //get bullet from the object pool
        //pool objects on level start
        //don't allow weapon swapping mid mission to avoid repooling mid level
        if (_canAttack) {
            GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
            if (bullet != null) {
                bullet.GetComponent<IProjectile>().SetFaction(faction);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                float deviation = Random.Range(-accuracy / 2, accuracy / 2); //a spray of bullets
                rb.velocity = transform.TransformDirection(new Vector3(deviation, 0, bullet.GetComponent<IProjectile>().GetVelocity()));
                bullet.transform.position = BarrelPoint.position;
                bullet.transform.rotation = GetComponentInParent<Transform>().rotation;
                _gunSounds.PlayOneShot(gunAudio[0]);
                bullet.GetComponent<ShortPistolBullet>().args.DamageValue = AttackValue;
                bullet.SetActive(true);
                CurMag--;
            }
            if (CurMag == 0) {
                Timing.RunCoroutine(Reload());
                _gunSounds.PlayOneShot(gunAudio[1]);
            }
            else {
                Timing.RunCoroutine(Delay());
            }
        }
    }

    //reset spin up
    public override IEnumerator<float> Reload(){
        AttackDelay = minSpinDelay;
        accuracy = accuracyMin;
        return base.Reload();
    }

    //activate spin up
    public override IEnumerator<float> Delay(){
        if (Mathf.Abs(AttackDelay - maxSpinDelay) > 0.002f){
            Mathf.Clamp(AttackDelay -= spinUpIncrement, maxSpinDelay, minSpinDelay);
        }
        if (Mathf.Abs(accuracy - accuracyMax) > 0.002f){
            Mathf.Clamp(accuracy += accuracyStep, accuracyMin, accuracyMax);
        }
        return base.Delay();
    }

    public override void ResetWeapon(){
        AttackDelay = minSpinDelay;
        accuracy = accuracyMin;
    }
}
