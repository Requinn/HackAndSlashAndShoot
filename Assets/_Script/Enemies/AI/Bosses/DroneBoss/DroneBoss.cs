using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using System;
using MEC;

public class DroneBoss : AIEntity {
    [Header("Boss Stats")]
    public Transform centerStage;
    [SerializeField]
    private List<DroneBossSummon> _droneList;
    private bool _isInAttack = false;
    private Queue<IEnumerator<float>> _queuedAttacks;
    private NavMeshAgent _navAgent;
    [SerializeField]
    private RotateConstant _droneRotator;
    [SerializeField]
    private float _movementCooldown = 2f;
    private float _moveCDDeviation = 0.5f;
    private float _moveCDAdjusted = 0f;
    private float _movementTimer = 0f; //start off moving
    private int _currentPhase = 0;

    [Header("Laser Attack")]
    [SerializeField]
    private DroneBossAttackValues _laserAttack;
    private float _laserTimer = 0f;

    [Header("Spray Attack")]
    [SerializeField]
    private DroneBossAttackValues _sprayAttack;
    private float _sprayTimer = 0f;

    [Header("Shotgun Attack")]
    [SerializeField]
    private DroneBossAttackValues _shotgunAttack;
    private float _shotgunTimer = 0f;

    // Use this for initialization
    void OnEnable () {
        _navAgent = GetComponent<NavMeshAgent>();
        _laserTimer = _laserAttack.cooldown[0] / 2f; //start the laser on half the cooldown
        _sprayTimer = _sprayAttack.cooldown[0] / 3f; //start the spray attack on a third of its cooldown
        TookDamage += CheckForPhase;
        AdjustAllWeapons();
        _queuedAttacks = new Queue<IEnumerator<float>>();
        Timing.RunCoroutine(AICycle());
	}

    /// <summary>
    /// check our health every time we get for phasing
    /// </summary>
    /// <param name="args"></param>
    private void CheckForPhase(Damage.DamageEventArgs args) {
        if(_currentPhase == 0 && HealthPercent() <= .8f) {
            _currentPhase = 1;
            AdjustAllWeapons();
        }
        if(_currentPhase == 1 && HealthPercent() <= .4f) {
            _currentPhase = 2;
            AdjustAllWeapons();
            _droneRotator.AdjustRotation(100f, -1);
        }
    }

    private void AdjustAllWeapons() {
        foreach(var drone in _droneList) {
            //drone.ModifyShotgun((int)_shotgunAttack.activeTime[_currentPhase]);
            drone.ModifySprayWeapon((int)_sprayAttack.activeTime[_currentPhase]);
        }
    }

    private IEnumerator<float> AICycle() {
        CoroutineHandle _currentAction;
        while (!IsDead) {
            if (_queuedAttacks.Count >= 1) {
                //run the routine
                _currentAction = Timing.RunCoroutine(_queuedAttacks.Peek());
                //wait for it to finish
                yield return Timing.WaitUntilDone(_currentAction);
                //remove it
                _queuedAttacks.Dequeue();
            }
            yield return 0f;
        }
    }

    // Update is called once per frame
    void Update () {
        UpdateTimers();
        //only queue up attacks if we have room
        if(_queuedAttacks.Count < 5) {
            if (_shotgunTimer >= _shotgunAttack.cooldown[_currentPhase]) {
                _queuedAttacks.Enqueue(ShotgunAttackAction());
                _shotgunTimer = 0f;
            }
            if (_laserTimer >= _laserAttack.cooldown[_currentPhase]) {
                _queuedAttacks.Enqueue(LaserAttackAction());
                _laserTimer = 0f;
            }
            if (_sprayTimer >= _sprayAttack.cooldown[_currentPhase]) {
                _queuedAttacks.Enqueue(SprayAttackAction());
                _sprayTimer = 0f;
            }
        }
        if(!IsDead && !_isInAttack) {
            Movement();
        }
	}

    private void UpdateTimers() {
        float curFrameTime = Time.deltaTime;
        _laserTimer += curFrameTime;
        _sprayTimer += curFrameTime;
        _shotgunTimer += curFrameTime;
        _movementTimer += curFrameTime;
    }

    protected override void Movement() {
        //if we have a path, and we're close enough, reset it to stop
        if(_navAgent.hasPath && _navAgent.remainingDistance <= _navAgent.stoppingDistance) {
            _navAgent.ResetPath();
            _moveCDAdjusted = _movementCooldown + UnityEngine.Random.Range(-_moveCDDeviation, _moveCDDeviation);
            _movementTimer = 0;
        }

        //if we don't have a path, make a new one
        if (!_navAgent.hasPath && _movementTimer >= _moveCDAdjusted) {
            //pick a point in a small circle 1/3rd the distance from the player to the boss i.e. [P-X---B], move there
            //distance from here to player
            //distance / 2 for radius of circle
            //player - radius for position of center of circle
            //pick random within that circle

            Transform player = GameController.Controller.PlayerReference.transform;
            float distance = Vector3.Distance(transform.position, player.position);
            Vector3 direction = (player.position - transform.position).normalized;
            float thirdDist = distance / 3;
            float radius = thirdDist / 2;
            Vector3 movePoint = UnityEngine.Random.insideUnitSphere + player.position * UnityEngine.Random.Range(0f, radius);
            movePoint.y = -1f; //this is floor height
            Debug.DrawLine(player.position, player.position + -direction * Mathf.Clamp(thirdDist, thirdDist, 6), Color.red);
            _navAgent.SetDestination(movePoint);
        }

    }

    /// <summary>
    /// simple attack action, all active drones fire a spray while they move
    /// </summary>
    private IEnumerator<float> SprayAttackAction() {
        foreach(var drone in _droneList) {
            if (!drone.isDisabled) {
                drone.FireSpray();
            }
        }
        yield return Timing.WaitForSeconds(2.5f);
        yield return 0f;
    }

    /// <summary>
    /// Fire a shotgun towards the player using the drone that matches the direction of the player
    /// </summary>
    private IEnumerator<float> ShotgunAttackAction() {
        int firedShots = 0;
        while (firedShots < _shotgunAttack.activeTime[_currentPhase]) {
            foreach (var drone in _droneList) {
                if (drone.isFacingPlayer && !drone.isDisabled) {
                    drone.isFacingPlayer = false;
                    drone.FireShotgun();
                    firedShots++;
                    break;
                }
            }
            if(_shotgunAttack.activeTime[_currentPhase] > 0) {
                yield return Timing.WaitForSeconds(0.5f);
            }
            yield return 0f;
        }
        yield return Timing.WaitForSeconds(1.2f);
        yield return 0f;
    }

    /// <summary>
    /// Head to the center of the arena, then fire a laser from all drones
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> LaserAttackAction() {
        _isInAttack = true;
        _navAgent.ResetPath();
        _navAgent.SetDestination(centerStage.position);
        while (_navAgent.pathPending) {
            yield return 0f;
        }
        while(_navAgent.remainingDistance > 0) {
            yield return 0f;
        }
        yield return Timing.WaitForSeconds(0.5f);
        //slow down rotation, go opposite direction
        _droneRotator.AdjustRotation(30, -1);
        foreach (var drone in _droneList) {
            if (!drone.isDisabled) {
                drone.FireLaser(_laserAttack.activeTime[_currentPhase]);
            }
        }
        yield return Timing.WaitForSeconds(_laserAttack.activeTime[_currentPhase] + 1f);
        _droneRotator.AdjustRotation(400f);
        yield return Timing.WaitForSeconds(1f);
        _droneRotator.AdjustRotation();
        _isInAttack = false;
        yield return 0f;
    }

    public override void ProjectileResponse() {
    }

    protected override void Attack() {
    }


}

[Serializable]
//Each array index is a phase value
public class DroneBossAttackValues {
    public float[] cooldown = new float[3];
    public float[] activeTime = new float[3];
}
