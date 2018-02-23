using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

/// <summary>
/// Bleeding effect, effects are too fast fix later
/// </summary>
public class Bleed : StatusObject{
    public float damagePerTick = 5;
    private float _initTime;
    private bool _canTick = false;

    void Start(){
        Timing.RunCoroutine(TickDown());
        InitializeProc();
        //StartProcs();
        _initTime = Time.time;
    }

    void Update(){
        if (_canTick && tickCount >= 0){
            GetComponentInParent<Entity>().TakeStatusDamage(damagePerTick, StatusType.Bleed);
            Timing.RunCoroutine(TickDown());
            tickCount--;
        }
        if (tickCount == 0){
            Destroy(gameObject);
        }
    }
    public void StartProcs(){
        _canTick = true;
        /**do{
            GetComponentInParent<Entity>().TakeStatusDamage(damagePerTick, Type.Bleed);
            Timing.RunCoroutine(TickDown());
        } while (_tickCount > 0);**/
    }

    public override IEnumerator<float> TickDown(){
        _canTick = false;
        yield return Timing.WaitForSeconds(tickDelay);
        _canTick = true;
    }
}
