using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enable and Disables an object every some seconds
/// </summary>
public class TimedObjectToggle : MonoBehaviour {
    [SerializeField]
    private float _timeOn = 5f;
    [SerializeField]
    private float _timeOff = 5f;
    [SerializeField]
    private bool _startDisabled = false;
    [SerializeField]
    private GameObject _object;

    private float _timeBetweenToggle = 0f;
    private float _currentTimer = 0f;

	void Start () {
        if (_startDisabled) {
            _object.SetActive(false);   
        }
	}
	
	void Update () {
        _currentTimer += Time.deltaTime;
        if(_currentTimer >= _timeBetweenToggle) {
            _currentTimer = 0f;
            if (_object.activeInHierarchy) {
                _timeBetweenToggle = _timeOff;
                _object.SetActive(false);
            }else {
                _timeBetweenToggle = _timeOn;
                _object.SetActive(true);
            }

        }
	}
}
