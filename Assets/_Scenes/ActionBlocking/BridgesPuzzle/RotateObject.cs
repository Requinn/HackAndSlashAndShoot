using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using System;

/// <summary>
/// Like TranslateObject, this script will apply a rotation of a certain amount to this object
/// </summary>
public class RotateObject : Toggleable {
    [SerializeField]
    private Vector3 _eulerRotation;
    [SerializeField]
    private bool _isPingPong = false; //does this rotation go back and forth? or does it continue forward until it does a 360?

    private bool _reverseRotation = false; //just used for pingpong movement

    //closing just does the same thing
    public override void Close() {
        Open();
    }

    public override void Open() {
        //just keep rotating forward
        if (!_isPingPong) {
            transform.Rotate(_eulerRotation);
        }else {
            //else check our direction, then apply rotation in that direction
            if (!_reverseRotation) {
                transform.Rotate(_eulerRotation);
            }
            else {
                transform.Rotate(-_eulerRotation);
            }
            _reverseRotation = !_reverseRotation;
        }
    }

}
