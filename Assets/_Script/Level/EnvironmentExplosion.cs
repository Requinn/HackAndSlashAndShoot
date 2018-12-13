using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add a force to all listed objects, and maybe entities as well?
/// </summary>
public class EnvironmentExplosion : MonoBehaviour {
    [SerializeField]
    private float _force = 5f; //how much force to apply to each object
    [SerializeField]
    private bool _canAffectEntities = false;
    [SerializeField]
    private float _explosionRadius = 10f;

    public Transform originPosition; //where is the force applied

    public List<Rigidbody> moveableObjects;

    private void OnEnable() {
        DoExplosion();
    }

    /// <summary>
    /// perform the explosion, and set each object to die after a couple of seconds
    /// </summary>
    public void DoExplosion() {
        foreach(Rigidbody moveable in moveableObjects) {
            moveable.AddExplosionForce(_force, originPosition.position, _explosionRadius, 0.75f, ForceMode.Impulse);
            Destroy(moveable.gameObject, 5.0f + Random.Range(-0.25f, 0.5f));
        }
    }
}
