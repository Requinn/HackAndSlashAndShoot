using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System;

/// <summary>
/// Provided two points, lerp between them
/// </summary>
public class LerpBetweenPoints : MonoBehaviour {
    [SerializeField]
    private Transform pointA, pointB;
    [SerializeField]
    private float _slideSpeed = 5f;
    [SerializeField]
    private float _timeAtA = 0f, _timeAtB = 0f; //how long should this platform spend at each point

    private Transform _currentTarget, _originPoint;
    private float _moveTimeStep = 0.0f;

    [SerializeField]
    private StartPosition _startPosition;

    private enum StartPosition {
        A, B
    };

	// Use this for initialization
	void Start () {
        if (_startPosition == StartPosition.A) {
            _originPoint = pointA;
            transform.position = pointA.position;
            _currentTarget = pointB;
        }else {
            transform.position = pointB.position;
            _originPoint = pointB;
            _currentTarget = pointA;
        }
        Timing.RunCoroutine(SlideObject());
	}

    private IEnumerator<float> SlideObject() {
        while (true) {
            //while we aren't at the targeted location, lerp there
            _moveTimeStep = 0f;
            while (!V3Equal(transform.position, _currentTarget.position)) {
                transform.position = Vector3.Lerp(_originPoint.position, _currentTarget.position, _moveTimeStep);
                _moveTimeStep += Time.deltaTime * _slideSpeed;
                yield return 0f;
            }
            //if we finish, swap target after we wait for the alotted time
            if (_currentTarget == pointA) {
                yield return Timing.WaitForSeconds(_timeAtA);
                _originPoint = pointA;
                _currentTarget = pointB;
            }
            else {
                yield return Timing.WaitForSeconds(_timeAtB);
                _originPoint = pointB;
                _currentTarget = pointA;
            }
            yield return 0f;
        }
    }

    private bool V3Equal(Vector3 a, Vector3 b) {
        return Vector3.Magnitude(a - b) < 0.001;
    }
}
