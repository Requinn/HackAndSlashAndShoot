using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using JLProject;
using JLProject.Weapons;
using MEC;
using UnityEngine;
using UnityEngine.UI;

public class MouseReticleReload : MonoBehaviour{
    [SerializeField]
    private Image fillImage;
    [SerializeField] private PlayerController player;
	// Use this for initialization
    void Start(){
        if (player == null){
            player = FindObjectOfType<PlayerController>();
        }
        player.swapped += AssignReloadEvent;
        player.equipped += AssignReloadEvent;
        fillImage.enabled = false;
    }

    // Update is called once per frame
	void Update (){
	    fillImage.transform.position = Input.mousePosition; //make sure we track the mouse
	}

    /// <summary>
    /// assigned the reload reticle animation to weapon just pickedup or swapped to
    /// </summary>
    void AssignReloadEvent(){
        player.CurrentWeapon.onReloadStart -= StartReloadAnimation; //unsub to make sure we're only triggering this once
        player.CurrentWeapon.onReloadStart += StartReloadAnimation;
    }

    private void AssignReloadEvent(Weapon primary, Weapon secondary) {
        player.CurrentWeapon.onReloadStart -= StartReloadAnimation;
        player.CurrentWeapon.onReloadStart += StartReloadAnimation;
    }


    void StartReloadAnimation(float t){
        Timing.RunCoroutine(PlayAnimation(t));
    }
    /// <summary>
    /// enable and fill the object in
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator<float> PlayAnimation(float time = 1f){
        fillImage.fillAmount = 0f;
        fillImage.enabled = true;
        while (fillImage.fillAmount < 1f){
            fillImage.fillAmount += Time.deltaTime / time;
            yield return 0f;
        }
        fillImage.enabled = false;
        yield return 0f;
    }
}
