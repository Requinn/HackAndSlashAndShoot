﻿using UnityEngine;
/// <summary>
/// Used only on objects that will be fixed to face the camera no matter the parent's rotation.
/// </summary>
public class FixedRotationToCamera : MonoBehaviour {
    Quaternion rotation;
    void Start() {
        rotation = Quaternion.identity;
        rotation.x = Camera.main.transform.rotation.x;
    }
    void LateUpdate() {
        transform.rotation = rotation;
    }
}
