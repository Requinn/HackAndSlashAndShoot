
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;
public class DamageZone : MonoBehaviour {
    [SerializeField]
    private bool CanAffectPlayer = true;
    [SerializeField]
    private bool CanAffectEnemy = false;
    [SerializeField]
    private bool AlwaysActive = true;
    [SerializeField]
    private bool _isTemporary = false;
    [SerializeField]
    private float _activeCooldown = 5.0f;
    [SerializeField]
    private float _warningDuration = 2.5f;
    [SerializeField]
    private float _activeDuration = 3.0f;

    public float TickTime = 3.0f;
    public float Damage = 15.0f;

    [SerializeField]
    public Damage.DamageType type = JLProject.Damage.DamageType.Neutral;

    private bool _canTick = true;

    private bool _isDestroyed = false;

    private Collider _collider;
    private Renderer _renderer;

    void Start(){
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<Renderer>();
        if (!AlwaysActive){
            Timing.RunCoroutine(CycleActivation());
            _collider.enabled = false;
            _renderer.material.color = Color.white; //temp stuff
        }
        if (AlwaysActive){
            _renderer.material.color = Color.red; //temp stuff
        }
        
    }

    /// <summary>
    /// If we aren't always active, cycle our active and deactive state while we are alive.
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> CycleActivation(){
        while (!_isDestroyed) {
            //start on cooldown
            yield return Timing.WaitForSeconds(_activeCooldown);

            //flash only when we decide to warn the player
            if (_warningDuration > 0.0f) {
                InvokeRepeating("FlashColor", 0f, 0.2f);
                yield return Timing.WaitForSeconds(_warningDuration);
                CancelInvoke("FlashColor");
            }

            //turn on
            ActivateDamage();

            //wait to turn off
            yield return Timing.WaitForSeconds(_activeDuration);
            DeactivateDamage();

            //if we are temporary, destroy after the zone is done being active
            if (_isTemporary) {
                Destroy(gameObject);
                _isDestroyed = true; //since we are awaiting deletion by the garbage collector, we have to exit our loop so we can peacefull await death
            }
        }
    }

    /// <summary>
    /// Flash between white and grey as a placeholder warning to player
    /// </summary>
    private void FlashColor(){
        if (_renderer.material.color != Color.white){
            _renderer.material.color = Color.white;
        }else if (_renderer.material.color != Color.gray){
            _renderer.material.color = Color.grey;
        }
    }

    private void ActivateDamage(){
        _renderer.material.color = Color.red;
        _collider.enabled = true;
    }

    private void DeactivateDamage(){
        _renderer.material.color = Color.white;
        _collider.enabled = false;
    }

    /// <summary>
    /// weak Heal method, expand to maybe heal enemies as well
    /// </summary>
    /// <param name="c"></param>
    void OnTriggerStay(Collider c) {
        if (c.GetComponent<Entity>() && _canTick) {
            PlayerController ent = c.gameObject.GetComponent<PlayerController>();
            if (ent) {
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
