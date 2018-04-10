using System.Collections.Generic;
using UnityEngine;

public abstract class StatusObject : MonoBehaviour{
    public float duration;
    public float tickDelay;
    public StatusType Type;

    public float tickCount;
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
    public abstract IEnumerator<float> TickDown();
}
