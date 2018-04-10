using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used to create an object that will respond to another item, such as a button, being interacted with
/// </summary>
public abstract class EventItem : MonoBehaviour{
    public abstract void Activate();
    public abstract void Deactivate();
}
