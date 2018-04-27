using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JLProject;
using UnityEngine;

public class ModMachineTrigger : MonoBehaviour{
    public ModMenu modUI;
    private bool interact = false;

    void Update(){
        if (interact && Input.GetKeyDown(KeyCode.E)){
            modUI.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider c){
        if (c.CompareTag("Player")){
            modUI.pc = c.GetComponent<PlayerController>();
            interact = true;
        }   
    }

    void OnTriggerExit(Collider c) {
        if (c.CompareTag("Player")) {
            interact = false;
        }
    }
}
