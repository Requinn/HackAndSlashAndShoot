using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;
using UnityEngine.Diagnostics;

public class WeaponPickup : MonoBehaviour {
    public GameObject WeaponToPickup;
    private float _pickupDelay = 5.0f;
    private bool _canPick = true;
    void OnTriggerStay(Collider c){
        if (c.gameObject.tag == "Player"){
            //display the pickup key
            if (Input.GetKeyDown(KeyCode.E) && _canPick){
                GameObject go = Instantiate(WeaponToPickup);
                c.gameObject.GetComponent<PlayerController>().Equip(go);
                Timing.RunCoroutine(PickUpDelay());
            }
        }
    }

    private IEnumerator<float> PickUpDelay(){
        _canPick = false;
        yield return Timing.WaitForSeconds(_pickupDelay);
        _canPick = true;
    }
}
