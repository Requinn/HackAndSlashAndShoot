using JLProject.Weapons;
using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LungeAndSlashWeapon : Melee {
    public override void Fire() {
        if (_canAttack) {
            Timing.RunCoroutine(TwiceAttack());
        }
    }

    private CoroutineHandle _hitboxDeactivateDelay;
    private IEnumerator<float> TwiceAttack() {
        if (_currentMag > 0 && _parentImpactRcvr) {
            PushForward(CurrentCombo);
        }
        _timeSinceSwing = 0.0f;

        //initial swing
        WaveComponent[0].SetActive(true);
        //This is to re enable the mesh, for some reason it turns off and stays off
        WaveComponent[0].GetComponent<MeshRenderer>().enabled = true;
        _hitboxDeactivateDelay = Timing.RunCoroutine(WaveDelay(0));
        yield return Timing.WaitUntilDone(_hitboxDeactivateDelay);

        yield return Timing.WaitForSeconds(0.15f); //tenth of a second between swings

        //second swing
        WaveComponent[1].SetActive(true);
        //This is to re enable the mesh, for some reason it turns off and stays off
        WaveComponent[1].GetComponent<MeshRenderer>().enabled = true;
        _hitboxDeactivateDelay = Timing.RunCoroutine(WaveDelay(1));

        CurMag--;
        //reload
        if (_currentMag == 0) {
            Timing.RunCoroutine(Reload());
        }
        yield return 0f;
    }
}
