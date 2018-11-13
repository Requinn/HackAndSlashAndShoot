using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using MEC;

public class MineLayer : AIEntity {
    [Header("MineLayer Properties")]
    [SerializeField]
    private float _minimumRunDistance = 3f;
    [SerializeField]
    private float _maximumRunDistance = 10f;
    [SerializeField]
    private float _turnAngle = 75f; //how far do we turn when we stop moving
    [SerializeField]
    private GameObject _mineToSpawn;
    [SerializeField]
    private Transform _mineSpawnLocation;

    private bool _wasLastActionLay = true; //start off as true so the first thing we do is move
    private CoroutineHandle _nextBehavior;

	// Use this for initialization
	new void Start () {
        base.Start();
        Timing.RunCoroutine(AICycle());
        _NMAgent.speed = MovementSpeed;
    }

    /// <summary>
    /// Pause, lay a mine, pause, go back to moving around
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> LayMine() {
        yield return Timing.WaitForSeconds(1f);
        Instantiate(_mineToSpawn, _mineSpawnLocation.position, Quaternion.identity);
        yield return Timing.WaitForSeconds(1f);
        _wasLastActionLay = true;
        yield return 0f;
    }

    /// <summary>
    /// Pick Left, if there's a wall, pick right, if wall, go forward, if wall, turn around 
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> RunToNextPosition() {
        //get a direction rotated bu our potential turning angle
        Vector3 newLocation = FindNextWayPoint();
        bool success = _NMAgent.SetDestination(newLocation);
        //while we move, stay here, or until our movement times out
        while(_NMAgent.velocity != Vector3.zero) {
            _wasLastActionLay = false;
            yield return 0f;
        }
        yield return 0f;
    }


    private float newX, newZ;
    /// <summary>
    /// Find the next location we are running to, we find one, return that, if not, stay still I guess
    /// </summary>
    /// <returns></returns>
    private Vector3 FindNextWayPoint() {
        Vector3 newPosition;
        float randomDist = Random.Range(_minimumRunDistance, _maximumRunDistance);

        //Checking the right angle
        newX = Mathf.Cos(_turnAngle) * transform.forward.x - Mathf.Sin(_turnAngle) * transform.forward.z;
        newZ = Mathf.Sin(_turnAngle) * transform.forward.x + Mathf.Cos(_turnAngle) * transform.forward.z;
        newPosition = new Vector3(newX, 0, newZ).normalized * randomDist;
        newPosition += transform.position; //add offset from our current position
        if (!Physics.Linecast(transform.position, newPosition, 1 << LayerMask.NameToLayer("Environment"))) {
            //since we didn't hit anything to our right
            return newPosition;
        }

        newX = Mathf.Cos(-_turnAngle) * transform.forward.x - Mathf.Sin(-_turnAngle) * transform.forward.z;
        newZ = Mathf.Sin(-_turnAngle) * transform.forward.x + Mathf.Cos(-_turnAngle) * transform.forward.z;
        newPosition = new Vector3(newX, 0, newZ).normalized * randomDist;
        newPosition += transform.position;
        if (!Physics.Linecast(transform.position, newPosition, 1 << LayerMask.NameToLayer("Environment"))) {
            //we didn't hit anything to our left
            return newPosition;
        }

        newPosition = transform.forward * randomDist;
        newPosition += transform.position;
        if (!Physics.Linecast(transform.position, newPosition, 1 << LayerMask.NameToLayer("Environment"))) {
            //we didn't hit anything in front of us
            return newPosition;
        }

        newPosition = -transform.forward * randomDist;
        newPosition += transform.position;
        if (!Physics.Linecast(transform.position, newPosition, 1 << LayerMask.NameToLayer("Environment"))) {
            //we didn't hit anything behind us
            return newPosition;
        }

        return transform.position;
    }

    private bool AreFloatsNearlyEqual(float a, float b, float tolerance) {
        return System.Math.Abs(a - b) <= tolerance;
    }

    private IEnumerator<float> AICycle() {
        while (!IsDead) {
            //choose what to do next, depending on what we just did
            if (_wasLastActionLay) {
                _nextBehavior = Timing.RunCoroutine(RunToNextPosition());
            }
            else {
                _nextBehavior = Timing.RunCoroutine(LayMine());
            }
            yield return Timing.WaitUntilDone(_nextBehavior);
            yield return 0f;
        }
    }

    public override void ProjectileResponse() {
        //nothing
    }

    protected override void Movement() {
        //nothing
    }

    protected override void Attack() {
        //nothing
    }
}
