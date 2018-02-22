using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

public class HealZone : MonoBehaviour{
    public bool CanAffectPlayer = true;
    public bool CanAffectEnemy = false;
    public float TickTime = 3.0f;
    public int Heal = 15;
    private bool _canTick = true;

    /// <summary>
    /// weak Heal method, expand to maybe heal enemies as well
    /// </summary>
    /// <param name="c"></param>
    void OnTriggerStay(Collider c){
        if (c.GetComponent<Entity>() && _canTick) {
            PlayerController ent = c.gameObject.GetComponent<PlayerController>();
            if (ent.gameObject.tag == "Player") {
                ent.Heal(this.gameObject, Heal);
                Timing.RunCoroutine(TickDelay(TickTime));
            }
        }
    }

    /// <summary>
    /// t delay inbetween ticks
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private IEnumerator<float> TickDelay(float t){
        _canTick = false;
        yield return Timing.WaitForSeconds(t);
        _canTick = true;
    }
}
