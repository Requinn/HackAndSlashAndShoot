using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

/// <summary>
/// basic turret code to seek a player and fire upon them
/// </summary>
public class Turret : MonoBehaviour{
    private FoVDetection _vision;
    public GameObject target;

	// Use this for initialization
	void Awake(){
	    _vision = GetComponent<FoVDetection>();
	    target = FindObjectOfType<PlayerController>().gameObject;
	}
	
	// Update is called once per frame
	void Update (){
	    if (_vision.CanSeeTarget(target.transform)){
	        Debug.Log("AAA");
	    }
	}
}
