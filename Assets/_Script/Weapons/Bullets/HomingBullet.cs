using JLProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MEC;

/// <summary>
/// This projectile will hard track targets
/// </summary>
public class HomingBullet : MonoBehaviour, IProjectile {
    public Damage.DamageEventArgs args;
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float _flightDistance = 10f;
    [SerializeField]
    private float _flightSpeed = 15f;
    [SerializeField]
    private StatusObject statusObj;

    private CoroutineHandle _repoolHandle;

    private RaycastHit hit;
    private Ray ray;

    private Vector3 _curPos;
    private float _lifetime;

    //homing stuff
    private float _distance;
    private Quaternion _angleToTarget;
    private Rigidbody _rb;
    private float _curveStr = 1f;

    public void SetTarget(GameObject T) {
        _target = T;
    }

    public void SetDamage(float damage) {
        args.DamageValue = damage;
    }

    public void Awake() {
        _rb = GetComponent<Rigidbody>();

    }

    public void Start() {
        if (!GameController.Controller.PlayerReference.IsDead) {
            if (GameController.Controller.PlayerReference.gameObject) {
                SetTarget(GameController.Controller.PlayerReference.gameObject);
            }
        }
    }
    public void OnEnable() {
        _curPos = transform.position;
        _lifetime = CalculateTimeToLive();
        _repoolHandle = Timing.RunCoroutine(DelayedRepool(_lifetime));
    }

    private float CalculateTimeToLive() {
        return _flightDistance / _flightSpeed;
    }


    //Use lateupdate so we can get the position of the target after it's potentially moved
    void LateUpdate() {
        //acquire direction to the target
        //change forward to match direction to object
        //rotational speed is decided by a value that will change how hard it will track the target
        //rotational speed is also scaled by the distance, maybe some kind of division so that short distances have tighter turns that long distances
        if (_target) {
            CalculateDistance();
            RotateToTarget();
            _rb.velocity = transform.forward * _flightSpeed; //update the velocity to match our new velocity after rotating
        }
    }

    /// <summary>
    /// Get the absolute value distance for scale purposes
    /// Get the direction of the target in relation to this projectile
    /// </summary>
    void CalculateDistance() {
        _distance = Mathf.Abs(Vector3.Distance(transform.position, _target.transform.position));
    }

    /// <summary>
    /// Get the angle between forward and direction to target
    /// then rotate object to that angle
    /// </summary>
    void RotateToTarget() {
        //get the angle to the target
        _angleToTarget = Quaternion.LookRotation(_target.transform.position - transform.position);

        //Debug.DrawRay(transform.position, transform.forward); //travel direction
        //Debug.DrawLine(transform.position, _target.transform.position); //Who is it going for

        //how strongly we rotate to the target, using speed to tighten curves, and distance to scale the speed so that closer objects have a tighter turn that further objects
        _curveStr = Mathf.Min(Time.deltaTime * (_flightSpeed / (_distance / 3)), 1);

        //aply the rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, _angleToTarget, _curveStr);
    }

    /// <summary>
    /// Check for collision
    /// </summary>
    /// <param name="hitObject"></param>
    void DoCollisionCheck(GameObject hitObject) {
        Entity ent = hitObject.GetComponent<Entity>();
        Switch swtch = hitObject.GetComponent<Switch>();
        BreakableObject brk = hitObject.GetComponent<BreakableObject>();
        //We did hit an entity
        if (ent != null) {
            //if we are the enemy, and what we hit was labeled an enemy, continue through them
            if (args.SourceFaction == Damage.Faction.Enemy && ent.Faction == Damage.Faction.Enemy) {
                return;
            }
            //if they aren't a player, or an ally
            if (ent.Faction != Damage.Faction.Player || ent.Faction != Damage.Faction.Allied) {
                //fix this later
                args.HitSourceLocation = transform.position;
                ent.TakeDamage(gameObject, ref args);
                if (statusObj) {
                    ApplyStatus(statusObj, ent);
                }
                Timing.KillCoroutines(_repoolHandle);
                gameObject.SetActive(false);
            }
            StopCoroutine(DelayedRepool(_lifetime));
        }
        else if (hitObject.gameObject.tag == "Environment") {
            Timing.KillCoroutines(_repoolHandle);
            gameObject.SetActive(false);
        }
        if (args.SourceFaction != Damage.Faction.Player) return;
        if (swtch) {
            swtch.Toggle();
            Timing.KillCoroutines(_repoolHandle);
            gameObject.SetActive(false);
        }
        if (brk) {
            brk.GetComponent<BreakableObject>().Hit();
            Timing.KillCoroutines(_repoolHandle);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Apply the status effect, refreshing if it exists
    /// </summary>
    /// <param name="SO"></param>
    public void ApplyStatus(StatusObject SO, Entity E) {
        StatusObject temp = E.Afflictions.Find(a => a.Type == SO.Type);
        if (temp) {
            temp.InitializeProc();
        }
        else {
            E.Afflictions.Add(Instantiate(SO, E.transform));
        }
    }

    public void OnTriggerEnter(Collider c) {
        DoCollisionCheck(c.gameObject);
    }

    //This actually means speed
    public float GetVelocity() {
        return _flightSpeed;
    }

    /// <summary>
    /// reset this projectile
    /// </summary>
    public void ResetLife() {
        Timing.KillCoroutines(_repoolHandle);
        _lifetime = CalculateTimeToLive();
        _repoolHandle = Timing.RunCoroutine(DelayedRepool(_lifetime));
    }

    /// <summary>
    /// repool bullet if its life is up
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private IEnumerator<float> DelayedRepool(float t) {
        yield return Timing.WaitForSeconds(t);
        Timing.KillCoroutines(_repoolHandle);
        gameObject.SetActive(false);
    }

    public void SetFaction(Damage.Faction f) {
        args.SourceFaction = f;
    }

    public Damage.Faction GetFaction() {
        return args.SourceFaction;
    }
}
