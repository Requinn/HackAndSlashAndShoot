﻿using System.Collections;
using System.Collections.Generic;
using JLProject;
using JLProject.Weapons;
using MEC;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

/// <summary>
/// Dispenses a weapon to the player.
/// </summary>
public class WeaponPickup : Interactable {
    public GameObject WeaponToPickup;
    public bool OneUse = false;
    public Image Icon;
    private PlayerController _pc;
    private float _pickupDelay = 5.0f;
    private bool _canPick = true;

    void Start(){
        UpdateIcon();
    }

    void Update(){
        if (_pc && Input.GetKeyDown(KeyCode.E) && _canPick) {
            GameObject go = Instantiate(WeaponToPickup);
            //only check for a replacement if we pick something different up
            if (_pc.CurrentWeapon && _pc.CurrentWeapon != go.GetComponent<Weapon>()){
                WeaponToPickup = ObjectReferencer.Instance.FetchObjByID(_pc.CurrentWeapon.ReferenceID);
                if(!OneUse) UpdateIcon();
            }
            _pc.Equip(go);
            InvokeInteractEvent();
            //alter the dispensing object to return the weapon the player was holding
             
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
            //Timing.RunCoroutine(CheckInput());
        }
    }

    void OnTriggerExit(Collider c){
        if (c.gameObject.CompareTag("Player")) {
            //disable the pickup key
            _pc = null;
        }
    }

    /// <summary>
    /// updates the displayed icon
    /// </summary>
    void UpdateIcon(){
        Icon.sprite = WeaponToPickup.GetComponent<Weapon>().weaponIcon;
    }

    /// <summary>
    /// more optimized that having update, not really necessary?
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> CheckInput(){
        while (_pc){
            if (_pc && Input.GetKeyDown(KeyCode.E) && _canPick) {
                GameObject go = Instantiate(WeaponToPickup);
                _pc.Equip(go);
                InvokeInteractEvent();
                if (OneUse) {
                    gameObject.SetActive(false);
                }
                else {
                    Timing.RunCoroutine(PickUpDelay());
                }
            }
            yield return 0f;
        }
        yield return 0f;
    }

    private IEnumerator<float> PickUpDelay(){
        _canPick = false;
        yield return Timing.WaitForSeconds(_pickupDelay);
        _canPick = true;
    }
}
