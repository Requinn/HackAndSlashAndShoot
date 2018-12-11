using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

/// <summary>
/// used to make enemies move around before aggroing on to the player.
/// Should be able to be attached to any enemy. 
/// When player is detected, this script disables.
/// Maybe develop this idea?
/// Idea is: this script disables base AI to make them patrol an area, once player is detected, disable this and re-enable the ai
/// Does'nt sound very clean?
/// </summary>
public class Patrol : MonoBehaviour {
    [SerializeField]
    private Transform[] _patrolPoints;
    private AIEntity _aiController;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
