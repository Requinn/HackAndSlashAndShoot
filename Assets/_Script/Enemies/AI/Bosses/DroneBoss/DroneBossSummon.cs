using MEC;
using UnityEngine;
using JLProject;
using System;
using System.Collections.Generic;

/// <summary>
/// Drone that is rottated around the boss, and told to fire off its weapons.
/// </summary>
public class DroneBossSummon : AIEntity {
    [Header("drone properties")]
    [SerializeField]
    private float _respawnTimer = 15f;
    [SerializeField]
    private SweepingLaser _laserWeapon;
    [SerializeField]
    private BurstGun _sprayGun;
    [SerializeField]
    private BulletHell _shotgun;

    public bool isFacingPlayer = false;
    public bool isDisabled = false;
    private MeshRenderer _visuals;
    private Collider _hitbox;
    private Transform _playerTransform; 

    new void Start() {
        base.Start();
        _hitbox = GetComponent<BoxCollider>();
        _visuals = GetComponent<MeshRenderer>();
        TookDamage += FlashColors; 
    }

    private void OnEnable() {
        _playerTransform = GameController.Controller.PlayerReference.transform;

    }

    /// <summary>
    /// Recalculate damage, drones take twice damage after armor from guns
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected override float CalculateDamage(Damage.DamageEventArgs e) {
        // reduce damage by ARMORVALUE% * 2.5x
        float damage = (int)Mathf.Clamp(e.DamageValue - (e.DamageValue * (_armorvalue * 0.01f)), 0, _maximumhealth);
        if(e.DamageType == Damage.DamageType.Ranged) {
            damage *= 2.5f;
        }
        if(e.DamageType == Damage.DamageType.Melee) {
            damage *= 0.9f;
        }
        return damage;
    }

    private void FlashColors(Damage.DamageEventArgs args) {
        Timing.RunCoroutine(FlashRoutine());
    }

    public void ModifySprayWeapon(int sprayCount) {
        _sprayGun.burstCount = sprayCount;
    }

    public void ModifyShotgun(int fireCount) {
        _shotgun.fireCount = fireCount;
    }

    public void FireLaser(float duration) {
        _laserWeapon.ActivateLaser(duration);
    }

    public void FireSpray() {
        _sprayGun.Fire();
    }

    public void FireShotgun() {
        _shotgun.Fire();
    }

    private void Update() {
        if(Vector3.Angle(transform.forward, _playerTransform.position - transform.position) <= 15f) {
            isFacingPlayer = true;
        }
        else {
            isFacingPlayer = false;
        }
    }

    /// <summary>
    /// instead of dying, we just disable the mesh and colliders for awhile
    /// </summary>
    protected override void HandleDeath() {
        _hitbox.enabled = false;
        _visuals.enabled = false;
        _laserWeapon.enabled = false;
        _shotgun.enabled = false;
        _sprayGun.enabled = false;
        isDisabled = true;
        Timing.RunCoroutine(WaitOutDeath());
    }

    /// <summary>
    /// Respawn
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> WaitOutDeath() {
        yield return Timing.WaitForSeconds(_respawnTimer);
        if (!this) { yield break; }
        CurrentHealth = MaxHealth;
        _hitbox.enabled = true;
        _visuals.enabled = true;
        _laserWeapon.enabled = true;
        _shotgun.enabled = true;
        _sprayGun.enabled = true;
        isDisabled = false;
        yield return 0f;
    }

    private IEnumerator<float> FlashRoutine() {
        float elapsedTime = 0f;
        while (elapsedTime < 0.1f) {
            if (!this) { yield break; }
            _visuals.material.color = Color.red;
            yield return Timing.WaitForSeconds(0.06f);
            _visuals.material.color = Color.white;
            yield return Timing.WaitForSeconds(0.06f);
            elapsedTime += Time.deltaTime;
        }
        yield return 0f;
    }


    public override void ProjectileResponse() {
        throw new NotImplementedException();
    }

    protected override void Movement() {
        throw new NotImplementedException();
    }

    protected override void Attack() {
        throw new NotImplementedException();
    }
}
