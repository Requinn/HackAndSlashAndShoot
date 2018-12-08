using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

public class AIMoveToPoint : MonoBehaviour {
    [SerializeField]
    private AIEntity _AI;
    public Vector3 TargetPoint;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            _AI.target = GameController.Controller.PlayerReference.gameObject;
        }
	}
}
