using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JLProject.Weapons;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// recieves the weapon swap event from the player to cycle the icons in the UI.
/// </summary>
namespace JLProject{
    public class WeaponIconHandler : MonoBehaviour{
        public Image primaryImg, secondaryImg;
        public Vector3 primaryScale = new Vector3(0.75f,0.75f,0.75f);
        public Vector3 secondaryScale = new Vector3(0.45f,0.45f,0.45f);
        public float visibleTime = 2.25f;
        private float _timeSeen;
        void Awake(){
            PlayerController p = FindObjectOfType<PlayerController>();
            p.swapped += SwapIcons;
            p.equipped += UpdateIcons;
            DisableIcons();
        }

        void Update(){
            if (_timeSeen <= visibleTime){
                _timeSeen += Time.deltaTime;
            }
            if (_timeSeen > visibleTime && primaryImg.IsActive() && secondaryImg.IsActive()){
                DisableIcons();
            }
        }

        /// <summary>
        /// Called when a new weapon is equipped, updates the Icons to match the weapon
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="secondary"></param>
        private void UpdateIcons(Weapon primary, Weapon secondary){
            primaryImg.sprite = primary.weaponIcon;
            if (secondary){
                secondaryImg.sprite = secondary.weaponIcon;
            }
            EnableIcons();
        }

        /// <summary>
        /// swaps the icons in the ui
        /// </summary>
        private void SwapIcons(){
            Vector3 posTemporary = secondaryImg.transform.position;
            Vector3 scaleTemporary = secondaryScale;
            secondaryImg.transform.DOMove(primaryImg.transform.position, 0.1f, false);
            secondaryImg.transform.DOScale(primaryScale, 0.1f);
            primaryImg.transform.DOMove(posTemporary, 0.1f, false);
            primaryImg.transform.DOScale(scaleTemporary, 0.1f);
            secondaryScale = primaryScale;
            primaryScale = scaleTemporary;
            EnableIcons();
        }

        private void EnableIcons(){
            primaryImg.gameObject.SetActive(true);
            secondaryImg.gameObject.SetActive(true);
            _timeSeen = 0f;
        }
        /// <summary>
        /// invoked to turn off the icons
        /// </summary>
        private void DisableIcons(){
            primaryImg.gameObject.SetActive(false);
            secondaryImg.gameObject.SetActive(false);
        }
    }
}
