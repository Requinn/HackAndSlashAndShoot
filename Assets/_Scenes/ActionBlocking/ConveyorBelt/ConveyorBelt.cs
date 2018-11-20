using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://www.gamasutra.com/blogs/MarkHogan/20141103/229201/Easy_Conveyor_Belt_Physics_in_Unity.php
/// Rigidbody based conveyor movement. Only works on objects that have a rigidbody component. Kind of useless for our character.
/// </summary>
public class ConveyorBelt : MonoBehaviour {
    [SerializeField]
    private float _pushSpeed = 2.0f;
    [SerializeField]
    private Vector3 _direction = new Vector3(1, 0, 0);
    private Rigidbody _rigidBody;

	// Use this for initialization
	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
        _direction = _direction.normalized;

    }
	
    //teleport object backwards, then use rigidbody movement to bring it forward, draggin objects in contact with it
	void FixedUpdate () {
        Vector3 deltaMove = _direction * _pushSpeed * Time.deltaTime;
        _rigidBody.position -= deltaMove;
        _rigidBody.MovePosition(_rigidBody.position + deltaMove);
    }
}
