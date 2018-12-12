using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using System;

public class ShieldDamageEventListener : LevelObjective {
    [SerializeField]
    private Entity _player;
    [SerializeField]
    private int _numberOfHits = 1;
    private int _hitCount = 0;

	// Use this for initialization
	void Start () {
        _player = GameController.Controller.PlayerReference;
        _player.TookShieldDamage += CheckTrigger; 
	}

    private void CheckTrigger(Damage.DamageEventArgs args) {
        _hitCount++;
        if(_hitCount >= _numberOfHits) {
            ObjectToUnlock.Open();
        }
    }
}
