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

    /// <summary>
    /// enable the object
    /// </summary>
    public override void Close() {
        _objectToToggle.SetActive(true);
    }

    /// <summary>
    /// disable the object
    /// </summary>
    public override void Open() {
        _objectToToggle.SetActive(false);
    }

}
