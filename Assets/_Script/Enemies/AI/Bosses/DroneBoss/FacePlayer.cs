using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

public class FacePlayer : MonoBehaviour {
    private Transform player;

	// Use this for initialization
	void OnEnable () {
        player = GameController.Controller.PlayerReference.transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(player);
	}
}
