using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using MEC;

public class MethodicalSlasher : AIEntity {
    [Header("Slasher Properties")]
    [SerializeField]
    private float _lungeRange = 10f; //How far awar can we leap at the player
    [SerializeField]
    private float _lungeCoolDown = 15f; //How often can we perform this lunge?
    [SerializeField]
    private float _minimumCircleDistance; //how close can we get during our circle movements
    [SerializeField]
    private float _maximumCircleDistance; //how far can we get during circle movements
    [SerializeField]
    private float _circlingSpeed = 10f; //how fast do we circle the player
    [SerializeField] 
    private float _circlingSpeedDeviation = 1.2f; // a slight randomness to the circling speed

    private float _currentCirclingDistance;
    private float _currentCirclingSpeed;
    private float _minCycleChangeTime = 3.5f, _maxCycleChangeTime = 8f; //how soon can we do a change in values for direction, speed etc?
    private float _timeSinceLastCycle = 0.0f;
    private float _currentCycleChangeTime = 0.0f;
    private float _timeSinceLunge = 0f;

    private List<CoroutineHandle> _handles = new List<CoroutineHandle>();
    private bool _isLungeReady = true;
    private Vector3 _originalLocation, _trackedPlayerLocation;

    protected new void Start() {
        base.Start();
        _handles.Add(Timing.RunCoroutine(AICycle()));
        _currentCirclingSpeed = _circlingSpeed;
        _currentCirclingDistance = _maximumCircleDistance;
        OnDeath += KillSelf;
    }

    /// <summary>
    /// When we die, disable all courtines on ourselves to stop running through a null guy
    /// TODO: potentially disables all other MethodicalSlashers
    /// </summary>
    private void KillSelf() {
        foreach(var h in _handles) {
            Timing.KillCoroutines(h);
        }
    }

    private CoroutineHandle LungeAttackHandle;

    public void Update() {
        //Lots of timer stuff
        if (!_isLungeReady) {
            _timeSinceLunge += Time.deltaTime;
            if(_timeSinceLunge >= _lungeCoolDown) {
                _isLungeReady = true;
            }
        }
        // a bit of randomness to our movements
        if(_timeSinceLastCycle <= _currentCycleChangeTime) {
            _timeSinceLastCycle += Time.deltaTime;
        }
        if(_timeSinceLastCycle > _currentCycleChangeTime) {
            _currentCycleChangeTime = Random.Range(_minCycleChangeTime, _maxCycleChangeTime);
            //change how close we are moving to the player
            _currentCirclingDistance = Random.Range(_minimumCircleDistance, _maximumCircleDistance);
            //change how fast we are going
            _circlingSpeed += Random.Range(-_circlingSpeedDeviation, _circlingSpeedDeviation);
            //25% to flip our movement direction
            if(Random.Range(0f, 100f) > 75f) {
                _circlingSpeed *= -1;
            }
            _timeSinceLastCycle = 0f;
        }
    }

    private IEnumerator<float> AICycle() {
        while (!IsDead) {
            //Debug.Log(_vision.CanSeeTarget(target.transform));
            if (_vision.CanSeeTarget(target.transform)) {
                Vector3 wallCheckLineOrigin = new Vector3(transform.position.x, transform.position.y - (_CC.height / 2) + 0.1f, transform.position.z); //line from our feet, to see if we can physically charge there
                Vector3 wallCheckLineDestination = new Vector3(target.transform.position.x, target.transform.position.y - (_CC.height / 2) + 0.1f, target.transform.position.z); //where our linecast is going
                //if within range, and we don't hit a wall
                if (_vision.GetDistanceToTarget <= _lungeRange && !Physics.Linecast(wallCheckLineOrigin, wallCheckLineDestination, 1 << LayerMask.NameToLayer("Environment"))) {
                    //and lunge is ready
                    if (_isLungeReady) {
                        //record where we are going and started
                        _trackedPlayerLocation = _vision.lastSeenPosition;
                        _originalLocation = transform.position;
                        //check for a wall between the player and here
                        LungeAttackHandle = Timing.RunCoroutine(LungeAttack());
                        _handles.Add(LungeAttackHandle);
                        //wait until we finish our lunge before returning to this AI cycle
                        yield return Timing.WaitUntilDone(LungeAttackHandle);
                    }
                    //and lunge is not ready
                    else {
                        //circle
                        Movement();
                    }
                }                    
                //if not within lunge range
                //move towards lunging range
                else {
                    transform.LookAt(target.transform);
                    _NMAgent.isStopped = false;
                    _NMAgent.SetDestination(target.transform.position); //go to the player
                }
            }
            yield return 0f;
        }
    }

    /// <returns></returns>
    private IEnumerator<float> LungeAttack() {
        _NMAgent.isStopped = true;
        CoroutineHandle dashHandle;
        //Math to make ourself land just short of the player
        //Simplifaction could be done by using the player's position +1 in a direction, rather than calculating distance - 1 in a direction
        Vector3 playerOffsetToSelf = _trackedPlayerLocation - transform.position; //get the offset
        float distanceToPlayer = playerOffsetToSelf.magnitude; //get distance
        Vector3 playerDistanceUnit = playerOffsetToSelf.normalized; //get direction
        Vector3 adjustedDistance = transform.position + playerDistanceUnit * (distanceToPlayer - 1f); //multiple distance by direction minus our stopping distance added to our position

        //"leap" at the player
        dashHandle = Timing.RunCoroutine(DashToLocation(adjustedDistance));
        yield return Timing.WaitUntilDone(dashHandle);
        //transform.LookAt(target.transform); //look at the player
        //attack
        Attack();
        yield return Timing.WaitForSeconds(.5f);
        //leap back
        dashHandle = Timing.RunCoroutine(DashToLocation(_originalLocation));
        yield return Timing.WaitUntilDone(dashHandle);
        yield return Timing.WaitForSeconds(0.1f);
        _isLungeReady = false;
        _timeSinceLunge = 0f;
        yield return 0f;
    }

    private IEnumerator<float> DashToLocation(Vector3 Location) {
        while(!Mathf.Approximately(transform.position.x, Location.x) && !Mathf.Approximately(transform.position.z, Location.z)) {
            transform.position = Vector3.Lerp(transform.position, Location, Time.deltaTime * 20f);
            yield return 0f;
        }
    }

    /// <summary>
    /// Circle the player
    /// </summary>
    protected override void Movement() {
        transform.LookAt(target.transform);
        transform.RotateAround(target.transform.position, Vector3.up, 5 * _currentCirclingSpeed * Time.deltaTime);
        Vector3 adjustedPosition = ((transform.position - target.transform.position).normalized * _currentCirclingDistance) + target.transform.position;
        transform.position = Vector3.Lerp(transform.position, adjustedPosition, Time.deltaTime * 3.5f);
        //_NMAgent.updatePosition = true; //maybe this will fix the jumpiness
    }

    protected override void Attack() {
        weapon.Fire();
    }

    public override void ProjectileResponse() { }
}