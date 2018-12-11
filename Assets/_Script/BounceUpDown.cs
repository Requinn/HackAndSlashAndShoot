using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move an object up and down between positions.
/// Starting position is where this object is located, moving up.
/// </summary>
public class BounceUpDown : MonoBehaviour {
    [SerializeField]
    private float _heightToMove = 1f, _bounceSpeed = .1f;
    private float _homePosition, _upPosition;
    private sbyte direction = 1;

	// Use this for initialization
	void Start () {
        _homePosition = transform.position.y;
        _upPosition = _homePosition + _heightToMove;
	}
	
	// Update is called once per frame
	void Update () {
        if (this) {
            if(transform.position.y <= _homePosition) {
                direction = 1;
            }
            else if(transform.position.y >= _upPosition) {
                direction = -1;
            }
            transform.position += new Vector3(0, direction * Time.deltaTime * _bounceSpeed, 0);
        }
	}
}
