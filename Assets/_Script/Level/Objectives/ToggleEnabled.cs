using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using System;

/// <summary>
/// toggles and object on or off
/// </summary>
public class ToggleEnabled : Toggleable{
    [SerializeField]
    private GameObject _objectToToggle;
    [SerializeField]
    [Tooltip("Calling OPEN will turn an object OFF. Turning this true will make OPEN turn the object ON.")]
    private bool _mirror = false; 
    /// <summary>
    /// enable the object
    /// </summary>
    public override void Close() {
        if (!_mirror) {
            _objectToToggle.SetActive(true);
        }
        else {
            _objectToToggle.SetActive(false);
        }
    }

    /// <summary>
    /// disable the object
    /// </summary>
    public override void Open() {
        if (!_mirror) {
            _objectToToggle.SetActive(false);
        }
        else {
            _objectToToggle.SetActive(true);
        }
    }

    public override void Toggle() {
        if (!_objectToToggle.activeInHierarchy) {
            _objectToToggle.SetActive(true);
        }
        else {
            _objectToToggle.SetActive(false);
        }
    }

}
