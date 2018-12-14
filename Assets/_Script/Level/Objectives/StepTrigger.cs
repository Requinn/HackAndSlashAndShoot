using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

public class StepTrigger : Interactable {
    public Toggleable toggleObject;
    public bool oneWay = true;

    private bool triggered = false;
    private void OnTriggerEnter(Collider other) {
        InvokeInteractEvent();
        if (!triggered) {
            toggleObject.Toggle();
            if (oneWay) {
                triggered = true;
            }
        }
    }
}
