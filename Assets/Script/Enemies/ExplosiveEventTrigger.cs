using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

/// <summary>
/// some badly written test code to trigger an explosion
/// </summary>
public class ExplosiveEventTrigger : EventItem{
    public Weapon explosive;
    public override void Activate(){
        explosive.Fire();
    }

    public override void Deactivate(){
        throw new System.NotImplementedException();
    }

    private IEnumerator<float> Delay() {
        yield return Timing.WaitForSeconds(0.025f);
        explosive.gameObject.SetActive(false);
    }
}
