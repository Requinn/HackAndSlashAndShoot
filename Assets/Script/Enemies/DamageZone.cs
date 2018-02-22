
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;
public class DamageZone : MonoBehaviour {
    public bool CanAffectPlayer = true;
    public bool CanAffectEnemy = false;
    public float TickTime = 3.0f;
    public float Damage = 15.0f;
    [SerializeField]
    public Damage.DamageType type = JLProject.Damage.DamageType.Neutral;

    private bool _canTick = true;

    /// <summary>
    /// weak Heal method, expand to maybe heal enemies as well
    /// </summary>
    /// <param name="c"></param>
    void OnTriggerStay(Collider c) {
        if (c.GetComponent<Entity>() && _canTick) {
            PlayerController ent = c.gameObject.GetComponent<PlayerController>();
            if (ent.gameObject.tag == "Player") {
                var args = new Damage.DamageEventArgs(Damage, c.transform.position);
                ent.TakeDamage(this.gameObject, ref args);
                Timing.RunCoroutine(TickDelay(TickTime));
            }
        }
    }

    /// <summary>
    /// t delay inbetween ticks
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private IEnumerator<float> TickDelay(float t) {
        _canTick = false;
        yield return Timing.WaitForSeconds(t);
        _canTick = true;
    }
}
