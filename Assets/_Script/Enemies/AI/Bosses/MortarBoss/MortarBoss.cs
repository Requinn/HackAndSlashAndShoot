using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class MortarBoss : AIEntity{
    public Transform activePosition; //position like invulnerable
    public Transform downedPosition; //position while vulnerable
    public SweepingLaser[] lasers;
    public BulletHell[] bullets;
    public BombardWeapon phase1Unique;

    private float bulletCooldown = 6f;
    private float laserCooldown = 12f;
    private float bulletCDstep = 2f; //how much we lower the bulelt hell cooldown by per phase;
    private float laserCDstep = 1f; //how often the laser cd is lowered per phase
    private int currentPhase = 1;

    private int phaseOneSweepDirection = 1;
	// Use this for initialization
	void OnEnable () {
		InvokeRepeating("FireLaser", 5f, laserCooldown);
        InvokeRepeating("FireBullets", 8f, bulletCooldown);
        InvokeRepeating("FireUnique", 25f, 20f);
	}

    /// <summary>
    /// Handles the sweeping Lasers
    /// </summary>
    private void FireLaser(){
        if (currentPhase == 1){
            lasers[1].StartSweep(90f * phaseOneSweepDirection, 180f * phaseOneSweepDirection, 10f);
            phaseOneSweepDirection = -phaseOneSweepDirection;
        }
        if (currentPhase == 2){
            lasers[0].StartSweep(-150f, 180f, 8f);
            lasers[2].StartSweep(150f, -180, 8f);
        }
        if (currentPhase == 3){
            //have one laser do a constant slow sweep while the two side ones do fast big sweeps
            lasers[1].StartSweep(90, 180f, 30f);
        }
    }

    /// <summary>
    /// handles the mini bullet hell
    /// </summary>
    private void FireBullets(){
        if (currentPhase == 1) {
            bullets[1].Fire();
        }
        if (currentPhase == 2) {
            lasers[0].StartSweep(-150f, 180f, 8f);
            lasers[2].StartSweep(150f, -180, 8f);
        }
        if (currentPhase == 3) {
            //have one laser do a constant slow sweep while the two side ones do fast big sweeps
            lasers[1].StartSweep(90, 180f, 30f);
        }
    }

    /// <summary>
    /// handles the unique attacks
    /// </summary>
    private void FireUnique(){
        if (currentPhase == 1){
            phase1Unique.delay = 0f;
            phase1Unique.Fire();
            phase1Unique.delay = 1f;
            phase1Unique.Fire();
        }
        if (currentPhase == 2) {
            lasers[0].StartSweep(-150f, 180f, 8f);
            lasers[2].StartSweep(150f, -180, 8f);
        }
        if (currentPhase == 3) {
            //have one laser do a constant slow sweep while the two side ones do fast big sweeps
            lasers[1].StartSweep(90, 180f, 30f);
        }
    }


    public override void ProjectileResponse(){
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
	void Update () {
		
	}

    protected override void Movement(){
        throw new System.NotImplementedException();
    }

    protected override void Attack(){
        throw new System.NotImplementedException();
    }
}
