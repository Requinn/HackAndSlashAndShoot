using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;
using UnityEngine.Diagnostics;

/// <summary>
/// Dispenses a weapon to the player.
/// </summary>
public class WeaponPickup : MonoBehaviour {
    public GameObject WeaponToPickup;
    public bool OneUse = false;
    private PlayerController _pc;
    private float _pickupDelay = 5.0f;
    private bool _canPick = true;

    void Update(){
        if (_pc && Input.GetKeyDown(KeyCode.E) && _canPick) {
            GameObject go = Instantiate(WeaponToPickup);
            _pc.Equip(go);

            if (OneUse){
                gameObject.SetActive(false);
            }
            else{
                Timing.RunCoroutine(PickUpDelay());
            }
        }
    }
    void OnTriggerEnter(Collider c){
        if (c.gameObject.CompareTag("Player")){
            //display the pickup key
            _pc = c.GetComponent<PlayerController>();
        }
    }

    void OnTriggerExit(Collider c){
        if (c.gameObject.CompareTag("Player")) {
            //disable the pickup key
        }
    }

    private IEnumerator<float> PickUpDelay(){
        _canPick = false;
        yield return Timing.WaitForSeconds(_pickupDelay);
        _canPick = true;
    }
}
