﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JLProject;
using UnityEngine;

/// <summary>
/// basic turret code to seek a player and fire upon them
/// </summary>
public class Turret : AIEntity{
    public float rotationTime = 3.0f;
    private Vector3 _pos, _dir;
    private Quaternion _lookrotation;

    // Use this for initialization
    void Awake(){
        base.target = FindObjectOfType<PlayerController>().gameObject;
    }

	// Update is called once per frame
	void Update (){
	}

    //this enemy does not move, but rotates
    protected override void Movement(){   
        _pos = transform.position;
        _dir = (target.transform.position - _pos).normalized;       //direction to look at
        _dir.y = 0;
        _lookrotation = Quaternion.LookRotation(_dir);              //generate a quaternion using the direction
        transform.DORotate(_lookrotation.eulerAngles, rotationTime);    //rotate towards it with a speed
    }

    protected override void Attack(){
        weapon.Fire();
    }

    public override void ProjectileResponse(){
        throw new System.NotImplementedException();
    }
}
