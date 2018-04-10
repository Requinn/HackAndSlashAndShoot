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
    private Entity _target;
    void Start(){
        _target = GetComponentInParent<Entity>();
        if (_target){
            Timing.RunCoroutine(TickDown());
        }
        InitializeProc();
        //StartProcs();
        _initTime = Time.time;
    }

    void Update(){

    }

    public void StartProcs(){
        if (_target) {
            Timing.RunCoroutine(TickDown());
        }
        /**do{
            GetComponentInParent<Entity>().TakeStatusDamage(damagePerTick, Type.Bleed);
            Timing.RunCoroutine(TickDown());
        } while (_tickCount > 0);**/
    }

    public override IEnumerator<float> TickDown(){
        while (tickCount >= 0){
            yield return Timing.WaitForSeconds(tickDelay);
            _target.TakeStatusDamage(damagePerTick, StatusType.Bleed);            
            tickCount--;
        }
        Destroy(gameObject);
    }
}
