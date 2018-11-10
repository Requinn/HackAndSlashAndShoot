using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

/// <summary>
/// Serves the weakpoint for any segmented enemy, able to take damage with modified values, and the option to break;
/// </summary>
public class WeakPoint : Entity{
    public bool isOpen = false;
    public bool isDestroyed = false;
    private BoxCollider _collider;

    void Start(){
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public delegate void WeakPointBreakEvent();
    public event WeakPointBreakEvent WeakPointBroken;

    /// <summary>
    /// How much damage do we pass onto the main body?
    /// </summary>
    /// <param name="damage"></param>
    public delegate void WeakPointDamagedEvent(Damage.DamageEventArgs damage);
    public event WeakPointDamagedEvent WeakPointDamaged;
    
    /// <summary>
    /// override to handle taking damage differently than other entities
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    public override void TakeDamage(object source, ref Damage.DamageEventArgs args) {
        //weakpoint is open
        if (isOpen){
            //we have a health, so we are breakable
            if (MaxHealth > 0f && CurrentHealth > 0f){
                CurrentHealth -= CalculateDamage(args);
                Mathf.Clamp(CurrentHealth, 0f, CurrentHealth);
                if (CurrentHealth == 0f){
                    WeakPointBroken();
                    HandleBreak();
                }
            }
            //we don't have health, so we serve as a criticial spot
            else{
                WeakPointDamaged(args);
            }
        }
    }

    /// <summary>
    /// Handle the case when we do break this weakpoint
    /// </summary>
    private void HandleBreak(){
        gameObject.SetActive(false);
        isDestroyed = true;
        isOpen = false;
    }

    /// <summary>
    /// open this weakspot, just make sure it isn't destroyed
    /// </summary>
    public void Open(){
        if (!isDestroyed){
            isOpen = true;
            _collider.enabled = true;
        }
    }

    protected override void Movement(){
        throw new System.NotImplementedException();
    }

    protected override void Attack(){
        throw new System.NotImplementedException();
    }
}
