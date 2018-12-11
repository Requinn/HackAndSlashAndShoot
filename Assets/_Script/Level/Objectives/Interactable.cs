using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour{
    public delegate void InteractEvent();
    public event InteractEvent Interracted;

    protected void InvokeInteractEvent(){
        if (Interracted != null) Interracted();
    }


}
