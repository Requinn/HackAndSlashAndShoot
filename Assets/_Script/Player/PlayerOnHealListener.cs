using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

public class PlayerOnHealListener : MonoBehaviour{
    private PlayerController _player;
    public GameObject healParticles;
    private EffectSettings _healParticleSettings;
    private float _timeSinceLastHealed = 0f;

	// Use this for initialization
	void Start (){
	    _healParticleSettings = healParticles.GetComponent<EffectSettings>();
	    _player = GetComponentInParent<PlayerController>();
	    _player.HealDamage += ActivateParticles;
	    SetHealParticle();
	}

    void Update(){
        //do a time since last healed to deactivate particles
        if (_timeSinceLastHealed < 2f){
            _timeSinceLastHealed += Time.deltaTime;
        }
        if (_timeSinceLastHealed >= 2f){
            if (healParticles.activeSelf){
                DeactivateParticles();
            }
        }
    }

    /// <summary>
    /// Set effect settings, unnecessary on this?
    /// </summary>
    private void SetHealParticle(){
    }

    /// <summary>
    /// activates healing particles
    /// </summary>
    /// <param name="amount"></param>
    private void ActivateParticles(float amount){
        healParticles.SetActive(true);
        _timeSinceLastHealed = 0f;
    }

    /// <summary>
    /// Deactivate them, look into a fade out or something
    /// </summary>
    private void DeactivateParticles(){
        healParticles.SetActive(false);
    }
}
