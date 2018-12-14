using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using JLProject;
using JLProject.Weapons;
using MEC;
using UnityEngine;

public class BurstGun : Gun{
    public int burstCount = 5;
    public float burstDelay = 0.05f;
    public float deviation = 0.1f; //lower is better
    [SerializeField]
    private bool _lockMovement = true;
    private AudioSource _gunSounds;
    public AudioClip[] gunAudio;
    public Transform BarrelPoint;
    private Damage.DamageEventArgs args;

    public delegate void BurstFiredEvent();

    private CoroutineHandle _delayHandle, _burstHandle;

    public event BurstFiredEvent BurstStart, BurstEnd;

    // Use this for initialization
    void Start(){
        _owningObj = GetComponentInParent<Entity>();
        _gunSounds = GetComponent<AudioSource>();
        if (faction == Damage.Faction.Enemy)
            bullet.objectToPool.tag = "EnemyProjectile";
        ObjectPooler.ObjectPool.PoolItem(bullet);
    }

    /// <summary>
    /// When the gun is disabled, as a result of being swapped out, stop it from shooting.
    /// </summary>
    private void OnDisable() {
        Timing.KillCoroutines(_burstHandle);
        if (_lockMovement && _owningObj) {
            _owningObj.ResetSpeed(); //burst over
        }
    }

    private IEnumerator<float> Burst() {
        //pre-emptively remove bullets from the magazine
        CurMag -= burstCount;
        for (int i = 0; i < burstCount;) {
            ReactivateBulletObj();
            yield return Timing.WaitForSeconds(burstDelay);
            i++;
        }
        if (CurMag <= 0) {
            Timing.KillCoroutines(_delayHandle);
            Timing.RunCoroutine(Reload());
            if (gunAudio.Length == 2) {
                _gunSounds.PlayOneShot(gunAudio[1]);
            }
        }
        if (_lockMovement) {
            _owningObj.ResetSpeed(); //burst over
        }
    }

    private void ReactivateBulletObj(){
        GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
        if (bullet != null){
            bullet.GetComponent<IProjectile>().SetFaction(faction);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            float accuracy = Random.Range(-deviation / 2, deviation / 2); //a spray of bullets
            rb.velocity = transform.TransformDirection(new Vector3(accuracy, 0, bullet.GetComponent<IProjectile>().GetVelocity()));
            bullet.GetComponent<IProjectile>().SetDamage(AttackValue);
            bullet.transform.position = BarrelPoint.position;
            bullet.transform.rotation = GetComponentInParent<Transform>().rotation;
            if (gunAudio.Length >= 1) {
                _gunSounds.PlayOneShot(gunAudio[0]);
            }
            bullet.SetActive(true);
        }
    }

    public override void Fire(){
        //get bullet from the object pool
        //pool objects on level start
        //don't allow weapon swapping mid mission to avoid repooling mid level
        if (_canAttack) {
            if (_lockMovement) {
                _owningObj.AdjustSpeed(0f); //can't move during a burst
            }
            _burstHandle = Timing.RunCoroutine(Burst());
            _delayHandle = Timing.RunCoroutine(Delay());
        }
    }

    /// <summary>
    /// fireoff a charged attack
    /// </summary>
    public override void ChargeAttack(){
        GameObject bullet = ObjectPooler.ObjectPool.GetPooledObject(base.bullet.objectToPool);
        if (bullet != null){
            bullet.GetComponent<IProjectile>().SetFaction(faction);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity =
                transform.TransformDirection(new Vector3(0, 0, bullet.GetComponent<IProjectile>().GetVelocity()));
            bullet.GetComponent<IProjectile>().SetDamage(AttackValue * burstCount);
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
    public override IEnumerator<float> Delay(){
        _canBlock = _canAttack = false;
        yield return Timing.WaitForSeconds(AttackDelay);
        _canBlock = _canAttack = true;
    }
}
