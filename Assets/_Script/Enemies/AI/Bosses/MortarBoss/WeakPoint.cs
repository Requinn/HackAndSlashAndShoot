using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves the weakpoint for the mortar boss
/// </summary>
public class WeakPoint : MonoBehaviour{
    public bool isOpen = false;
    public bool isDestroyed = false;

    private BoxCollider collider;

    void Start(){
        collider.enabled = false;
    }

    public delegate void WeakPointHitEvent();
    public event WeakPointHitEvent WeakPointBroken;

    public void GetHit(){
        if (isOpen){
            WeakPointBroken();
        }
    }

    public void Open(){
        isOpen = true;
        collider.enabled = true;
    }
}
