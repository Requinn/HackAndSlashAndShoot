using System;
using JLProject;
using UnityEngine;


public class AoESlowBullets : AIEntity {
    [Header("Slow Stats")]
    [SerializeField]
    private DamageZone _damageZone;
    [SerializeField]
    private HealZone _healZone; //maybe?
    [SerializeField]
    private float _activationRadius = 6f; //this should match, or be a little less? than the actual area or something, not something super tied to anything

    private Collider[] _overlappedObjects;
    private int playerLayerMask = 1 << 10; //used as a filter to only care about the player object in the overlapped objects array.

    new void Start()
    {
        base.Start();
        if (_damageZone)
        {
            _damageZone.gameObject.SetActive(false);
        }
    }

    public void FixedUpdate()
    {
        if (_damageZone)
        {
            _overlappedObjects = Physics.OverlapSphere(transform.position, _activationRadius, playerLayerMask);
            //if the overlap sphere detected the player
            if (_overlappedObjects.Length > 0)
            {
                _damageZone.gameObject.SetActive(true);
            }
            else
            {
                _damageZone.gameObject.SetActive(false);
            }
        }
    }
    public override void ProjectileResponse()
    {
        throw new NotImplementedException();
    }

    protected override void Attack()
    {
        //If within range, do the attack
    }

    protected override void Movement()
    {
        //This noodle does not move
    }

}
