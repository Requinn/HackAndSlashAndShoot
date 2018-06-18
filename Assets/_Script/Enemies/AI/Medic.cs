using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

/// <summary>
/// Medic Enemy WIP
/// </summary>
public class Medic : MeleeUnit{
    public HealingWeapon healTool;
    public float lowHealthThreshold = .35f;
    public float thresholdPercent = .50f; //how many targets out of all in range have low health
    public int thresholdCount;

   new void Start(){
        InvokeRepeating("CheckNearbyAllies", 0f, 3f);
        OnDeath += StopChecking;
    }
    /// <summary>
    /// checks all allies in range for health
    /// </summary>
    protected void CheckNearbyAllies(){
        thresholdCount = 0;
        if (healTool.targets.Count > 0 && !IsDead){
            foreach (var ally in healTool.targets){
                if (ally.HealthPercent() <= lowHealthThreshold){
                    thresholdCount++;
                }
            }
            if ((float)thresholdCount / (float)healTool.targets.Count >= thresholdPercent){
                if(!IsDead) healTool.Fire();
            }
        }
    }

    void StopChecking(){
        CancelInvoke("CheckNearbyAllies");
    }

    protected override void HandleDeath(){
        onDeath();
        IsDead = true;
        gameObject.SetActive(false);
        healTool.gameObject.SetActive(false);
    }
}
