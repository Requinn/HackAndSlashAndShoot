using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstant : MonoBehaviour {
    [SerializeField]
    private float _rotationSpeed = 90f; //degrees per second
    [SerializeField]
    private short _direction = 1;
    private float originalRotation;
    private short originalDirection;

    private void Start() {
        originalDirection = _direction;
        originalRotation = _rotationSpeed;
    }

    /// <summary>
    /// Adjust rotational values. Call blank paramters to reset to riginal values
    /// </summary>
    /// <param name="newSpeed"></param>
    /// <param name="newDirection"></param>
    public void AdjustRotation(float newSpeed = 90f, short newDirection = 1) {
        _rotationSpeed = newSpeed;
        _direction = newDirection;
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(0, _rotationSpeed * _direction * Time.deltaTime, 0);
	}
}
