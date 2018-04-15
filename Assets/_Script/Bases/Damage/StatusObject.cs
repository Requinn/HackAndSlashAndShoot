using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

public abstract class StatusObject : MonoBehaviour{
    public float duration;
    public float tickDelay;
    public StatusType Type;
    private Entity _target;
    public float tickCount;
    public float damagePerTick = 5;
    private float _initTime;
    private bool _canTick = false;

    void Start() {
        _target = GetComponentInParent<Entity>();
        if (_target) {
            Timing.RunCoroutine(TickDown());
        }
        InitializeProc();
        //StartProcs();
        _initTime = Time.time;
    }

    public void InitializeProc() {
        tickCount = duration / tickDelay;
    }
    [SerializeField]
    public enum StatusType{
        Bleed,
        Poison,
        Shock,
        Freeze
    }

    public void StartProcs() {
        if (_target) {
            Timing.RunCoroutine(TickDown());
        }
        /**do{
            GetComponentInParent<Entity>().TakeStatusDamage(damagePerTick, Type.Bleed);
            Timing.RunCoroutine(TickDown());
        } while (_tickCount > 0);**/
    }

    public IEnumerator<float> TickDown() {
        while (tickCount >= 0) {
            yield return Timing.WaitForSeconds(tickDelay);
            _target.TakeStatusDamage(damagePerTick, Type);
            tickCount--;
        }
        _target.Afflictions.Remove(this);
        Destroy(gameObject);
    }
}
