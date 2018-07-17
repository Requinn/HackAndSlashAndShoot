using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JLProject;
using MEC;
using NUnit.Framework.Constraints;
using UnityEngine;

/// <summary>
/// Boss Description: A large body that can be hit for half damage the whole fight, after hitting a certain threshold, the player must activate a mortar to 
/// break a weakpoint, and expose the head, allowing a brief window of increased damage until the next phase
/// </summary>
public class MortarBoss : AIEntity{
    public Transform activePosition; //position like invulnerable
    public Transform downedPosition; //position while vulnerable
    public SweepingLaser[] lasers;
    public BulletHell[] bullets;
    public BombardWeapon phase1Unique;
    public BombardWeapon phase2Unique;
    public Explosive roomNuke;
    public WeakPoint[] vulnSpot;
    public WeakPoint HeadHitBox; //hitbox used for the head, will take more damage
    private BoxCollider _headCollider;
    public float roomNukeTimer = 10f;
    public float HeadExposureTime = 10f;
    private float bulletCooldown = 6f;
    private float laserCooldown = 12f;
    private float bulletCDstep = 2f; //how much we lower the bulelt hell cooldown by per phase;
    private float laserCDstep = 1f; //how often the laser cd is lowered per phase
    private bool _interrupted = false; //did we get interrupted
    public float _timeInChannel = 0f; //how long have we spent doing our phase transition
    public int currentPhase = 0;

    private Explosive _instantiatedNuke;
    public float[] phaseThresholds; //what health do we change phases at

    private int phaseOneSweepDirection = 1;

    /// <summary>
    /// Used to tell anything in the level, i.e. the switches, the weakpoints are open, in order to add a visual cue
    /// Duration is how long we have until the bomb goes off
    /// </summary>
    public delegate void WeakPointExposedEvent(float duration);
    public WeakPointExposedEvent WeakPointExposed;

    public delegate void WeakPointCoveredEvent();
    public WeakPointCoveredEvent WeakPointCovered;

    // Use this for initialization
    void OnEnable (){
	    EnableAllAttacks();
		
	    TookDamage += CheckPhase;
	    HeadHitBox.WeakPointDamaged += TransferDamageToBody;

	}

    void Start(){
        base.Start();
        roomNuke.CastTime = roomNukeTimer;
        _headCollider = HeadHitBox.GetComponent<BoxCollider>();
    }

    /// <summary>
    /// call the repeat invoke on all the attacks
    /// </summary>
    private void EnableAllAttacks(){
        InvokeRepeating("FireLaser", 5f, laserCooldown);
        InvokeRepeating("FireBullets", 8f, bulletCooldown);
        InvokeRepeating("FireUnique", 25f, 20f);
    }

    /// <summary>
    /// stop doing attacks
    /// </summary>
    private void CancelAllAttacks(){
        CancelInvoke("FireLaser");
        CancelInvoke("FireBullets");
        CancelInvoke("FireUnique");

    }
    /// <summary>
    /// Called by event to apply damage to the main body when the head is down
    /// Damage is calculated on the hitbox and sent through an event to here
    /// </summary>
    /// <param name="args"></param>
    private void TransferDamageToBody(Damage.DamageEventArgs damage){
        TakeRawDamage(this, ref damage);
        CheckPhase();
    }

    /// <summary>
    /// check our health every hit to change phase
    /// </summary>
    /// <param name="damage"></param>
    private void CheckPhase(Damage.DamageEventArgs damage = default(Damage.DamageEventArgs)){
        if (currentPhase < phaseThresholds.Length){ //check to make sure we don't go to a third threshold for this  3 phase fight
            if (HealthPercent() <= phaseThresholds[currentPhase]){
                currentPhase++;
                StartVulnerability(currentPhase);
            }
        }
    }

    /// <summary>
    /// open any weakpoints that haven't been destroyed
    /// </summary>
    /// <param name="phase"></param>
    void StartVulnerability(int phase){
        Debug.Log("entering Vuln");
        CancelAllAttacks();
        StartCastingWipe();
        WeakPointExposed(roomNukeTimer);
        foreach (var v in vulnSpot) {
            if (!v.isDestroyed) {
                v.Open();
                v.WeakPointBroken += RecieveWeakPoint;
            }
        }
    }

    /// <summary>
    /// BigRoomAoE wipe if you fail the mechanic to phase transition
    /// </summary>
    private void StartCastingWipe(){
        _instantiatedNuke = Instantiate(roomNuke, transform.position + new Vector3(5,0,-5), Quaternion.identity); //created at the offset center
        _instantiatedNuke.CastTime = roomNukeTimer;
        _instantiatedNuke.Fire();
        Timing.RunCoroutine(CheckForInterruption());
    }

    /// <summary>
    /// Check and handle interruptions in the casting of the nuke
    /// This should fix multifiring weapons on later phases, and possibly the lasers
    /// </summary>
    private IEnumerator<float> CheckForInterruption(){
        Debug.Log("Starting wipe cast");
        _timeInChannel = 0f;
        //wait until we finish casting to provoke a missed call
        while (_timeInChannel < _instantiatedNuke.CastTime){
            float wait = Time.deltaTime;
            _timeInChannel += wait;
            //unless we do get interrupted then leave
            if (_interrupted) {
                break;
            }
            yield return Timing.WaitForSeconds(wait); //fake update cycles out of update()
        }
        Debug.Log(_interrupted);
        if (!_interrupted) {
            Invoke("MissedWeakPoint", _instantiatedNuke.CastTime);
        }
        if (_interrupted){
            _interrupted = false; //toggle the bool to false and leave
        }
        yield return 0f;
    }

    /// <summary>
    /// recieve a hit from the weakpoint, bomb is gone, get free damage
    /// </summary>
    public void RecieveWeakPoint(){
        _interrupted = true;
        foreach (var v in vulnSpot){
            v.WeakPointBroken -= RecieveWeakPoint; //unsub from the weakpoints to prevent accidents
        }
        WeakPointCovered();
        _instantiatedNuke.Extinguish(); //cancel the roomnuke
        Timing.RunCoroutine(ExposeHead()); //drop the head
    }

    /// <summary>
    /// we didn't hit the mortar in time, bomb goes off and we miss free damage
    /// </summary>
    private void MissedWeakPoint(){
        foreach (var v in vulnSpot) {
            v.WeakPointBroken -= RecieveWeakPoint; //unsub from the weakpoints to prevent accidents
        }
        Debug.Log("enabled as a miss");
        EnableAllAttacks();
    }

    /// <summary>
    /// Expose the head hitbox for X seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> ExposeHead(){
        _headCollider.enabled = true;
        HeadHitBox.transform.DOMove(downedPosition.position, 1.2f, false); //move head back down
        yield return Timing.WaitForSeconds(HeadExposureTime);
        HeadHitBox.transform.DOMove(activePosition.position, 2f); 
        _headCollider.enabled = false;
        EnableAllAttacks();
    }

    /// <summary>
    /// Handles the sweeping Lasers
    /// </summary>
    private void FireLaser(){
        if (currentPhase == 0){
            lasers[1].StartSweep(90f * phaseOneSweepDirection, 180f * phaseOneSweepDirection, 10f);
            phaseOneSweepDirection = -phaseOneSweepDirection;
        }
        if (currentPhase == 1){
            lasers[0].StartSweep(-150f, 181f, 8f);
            lasers[2].StartSweep(150f, -181f, 8f);
            phaseOneSweepDirection = -phaseOneSweepDirection;
        }
        if (currentPhase == 2){
            //have one laser do a constant slow sweep while the two side ones do fast big sweeps
            lasers[1].StartSweep(90, 180f, 30f);
        }
    }

    /// <summary>
    /// handles the mini bullet hell
    /// </summary>
    private void FireBullets(){
        bullets[currentPhase].Fire();
    }

    /// <summary>
    /// handles the unique attacks
    /// </summary>
    private void FireUnique(){
        if (currentPhase == 0){
            phase1Unique.delay = 0f;
            phase1Unique.Fire();
            phase1Unique.delay = 1f;
            phase1Unique.Fire();
        }
        if (currentPhase == 1) {
            phase2Unique.Fire();
        }
        if (currentPhase == 2) {
            //nothing yet
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
