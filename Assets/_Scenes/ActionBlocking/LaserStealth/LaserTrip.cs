using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

/// <summary>
/// For now, just used to rander a line forward until it hits something?
/// </summary>
[ExecuteInEditMode]
public class LaserTrip : MonoBehaviour {
    [SerializeField]
    private float _laserMaxLength = 10f;

    public LevelObjective Objective;

    private LineRenderer _LineRender;
    private Vector3 _endPoint;
    private Ray _forwardRay;
    private RaycastHit _laserEnd;

    private void Start() {
        if (!GetComponent<LineRenderer>()) {
            gameObject.AddComponent<LineRenderer>();
        }
        _LineRender = GetComponent<LineRenderer>();
        _LineRender.SetPosition(0, transform.position); //set home souce to be on us
        _forwardRay = new Ray(transform.position, transform.forward);
    }

    // Update is called once per frame
    void Update () {
        _LineRender.SetPosition(0, transform.position); //set home souce to be on us
        _forwardRay = new Ray(transform.position, transform.forward);
        //send out a raycast every frame to find if we hit anything
        if (Physics.Raycast(_forwardRay, out _laserEnd, _laserMaxLength)) {
            //laser was tripped by the player, start the objective if isn't already happning or completed.
            if(_laserEnd.collider.tag == "Player" && Objective && (!Objective.isActiveAndEnabled || !Objective.isCompleted)) {
                Objective.Initiate();
            }
            _endPoint = _laserEnd.point;
        }
        //if we don't just go on til the max range of the laser
        else {
            _endPoint = transform.forward * _laserMaxLength + transform.position;
        }
        _LineRender.SetPosition(1, _endPoint);
	}
}
