using JLProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to add the functionality that allows this object to reparent entities that enter this collider so that it may move it with them. I.E. Moving platforms.
/// </summary>
public class ReparentOnCollision : MonoBehaviour {

    /// <summary>
    /// On entering the collider, set the parent of the entering objet to this object
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        //dirty check for bullets and melee hitboxes
        if(other.GetComponent<IProjectile>() != null || other.GetComponent<WaveCollision>()) { return; }
        other.transform.SetParent(transform);
    }

    /// <summary>
    /// Release the object
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<IProjectile>() != null) { return; }
        //if we leave the collider and the root parent is this object, then remove it, if not, then leave it alone
        //fixes previous object resetting parent when moving directly into a new collider
        if (other.transform.root.gameObject == this.transform.root.gameObject) {
            other.transform.SetParent(null);
        }
    }
}
