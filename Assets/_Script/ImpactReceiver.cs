using UnityEngine;

using System.Collections;
using JLProject;

/// <summary>
/// adds impact to the character that has this script
/// called by AddImpact(direction, force);
/// </summary>
public class ImpactReceiver : MonoBehaviour {
    public float mass = 3.0F; // defines the character mass
    Vector3 impact = Vector3.zero;
    private CharacterController _character;
    private PlayerController _playerRef;
    // Use this for initialization
    void Start() {
        _character = GetComponent<CharacterController>();
        _playerRef = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        // apply the impact force:
        if (impact.magnitude > 0.2F && _character.enabled) _character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }
    
    /// <summary>
    /// Apply a force in a direction
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="force"></param>
    public void AddImpact(Vector3 dir, float force){
        //If we're blocking, halve the force
        if (_playerRef && _playerRef.CurrentShield.blocking){
            force = force / 2;
        }
        dir.Normalize();
        if (dir.y > 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }
}