using JLProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshDashTest : MonoBehaviour {
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(GameController.Controller.PlayerReference.transform);
        if (Input.GetKey(KeyCode.E)) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(40, 1.5f, -18), Time.deltaTime * 10f);
        }
        if (Input.GetKey(KeyCode.R)) {
            agent.isStopped = true;
            agent.Move(Time.deltaTime * 4 * transform.forward);
        }
	}

    void SetDestination() {
        
    }
}
