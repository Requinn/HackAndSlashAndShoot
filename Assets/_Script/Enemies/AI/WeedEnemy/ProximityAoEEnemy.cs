using System;
using JLProject;
using UnityEngine;

/// <summary>
/// Idea for consideration?
/// Different bubbles around it, maybe have a healing one for enemies who are coincidentally near it (no smart tracking or guiding them here)
/// Blocking/Reflecting Bubble with a projectile or something?
/// 
/// No health bar? just to add a sort of design that is "The fuck is this thing? oh it hurts!"
/// </summary>
public class ProximityAoEEnemy : AIEntity {
    [Header("Weed Statistics")]
    [SerializeField]
    private DamageZone _damageZone;
    [SerializeField]
    private HealZone _healZone; //maybe?
    [SerializeField]
    private float _activationRadius = 6f; //this should match, or be a little less? than the actual area or something, not something super tied to anything

    private Collider[] _overlappedObjects;
    private int playerLayerMask = 1 << 10; //used as a filter to only care about the player object in the overlapped objects array.

    new void Start() {
        base.Start();
        _damageZone.gameObject.SetActive(false);
    }

    public void FixedUpdate() {
        _overlappedObjects = Physics.OverlapSphere(transform.position, _activationRadius, playerLayerMask);
        //if the overlap sphere detected the player
        if (_overlappedObjects.Length > 0) {
            _damageZone.gameObject.SetActive(true);
        }
        else {
            _damageZone.gameObject.SetActive(false);
        }
    }
    public override void ProjectileResponse() {
        throw new NotImplementedException();
    }

    protected override void Attack() {
        //If within range, do the attack
    }

    protected override void Movement() {
        //This noodle does not move
    }

}
