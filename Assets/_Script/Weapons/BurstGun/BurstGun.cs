using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject;
using MEC;
using UnityEngine;

public class BurstGun : Gun {
    public int burstCount = 5;
    public float burstDelay = 0.05f;
    public float spray = 0.1f;
    private AudioSource _gunSounds;
    public AudioClip[] gunAudio;
    public Transform BarrelPoint;
    private Damage.DamageEventArgs args;
    public delegate void BurstFiredEvent();

    private CoroutineHandle _delayHandle;
    public event BurstFiredEvent BurstStart, BurstEnd;
    // Use this for initialization
    void Start(){
        _owningObj = GetComponentInParent<Entity>();
        _gunSounds = GetComponent<AudioSource>();
        if (faction == Damage.Faction.Enemy)
            bullet.objectToPool.tag = "EnemyProjectile";
        if (ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool) == null){
            ObjectPooler.ObjectPool.PoolItem(bullet);
        }
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
        _owningObj.ResetSpeed(); //burst over
    }

    private void ReactivateBulletObj(){
        GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
        if (bullet != null) {
            bullet.GetComponent<IProjectile>().SetFaction(faction);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            float deviation = Random.Range(-spray/2, spray/2); //a spray of bullets
            rb.velocity = transform.TransformDirection(new Vector3(deviation, 0, bullet.GetComponent<IProjectile>().GetVelocity()));
            bullet.GetComponent<ShortPistolBullet>().args.DamageValue = AttackValue;
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
            _owningObj.AdjustSpeed(0f); //can't move during a burst
            Timing.RunCoroutine(Burst());
            _delayHandle = Timing.RunCoroutine(Delay());
        }
    }

    /// <summary>
    /// fireoff a charged attack
    /// </summary>
    public override void ChargeAttack() {
        GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
        if (bullet != null) {
            bullet.GetComponent<IProjectile>().SetFaction(faction);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = transform.TransformDirection(new Vector3(0, 0, bullet.GetComponent<IProjectile>().GetVelocity()));
            bullet.GetComponent<ShortPistolBullet>().args.DamageValue = (AttackValue * burstCount);
            bullet.transform.position = BarrelPoint.position;
            bullet.transform.rotation = GetComponentInParent<Transform>().rotation;
            _gunSounds.PlayOneShot(gunAudio[0]);
            bullet.SetActive(true);
        }
        Timing.RunCoroutine(base.Reload());
        _gunSounds.PlayOneShot(gunAudio[1]);
    }

    /// <summary>
    /// the delay between attacks
    /// </summary>
    /// <returns></returns>
    public override IEnumerator<float> Delay() {
        _canBlock = _canAttack = false;
        yield return Timing.WaitForSeconds(AttackDelay);
        _canBlock = _canAttack = true;
    }
}
