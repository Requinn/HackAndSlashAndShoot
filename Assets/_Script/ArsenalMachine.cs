using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject.Weapons;
using UnityEngine.UI;
using System;

namespace JLProject {
    /// <summary>
    /// Machine placed in the world to access all weapons and such in one spot
    /// </summary>
    public class ArsenalMachine : MonoBehaviour {
        [SerializeField]
        private Weapon[] _weapons;
        [SerializeField]
        private Shield[] _shields;

        [Header("WeaponsMenu")]
        public Button[] _weaponChoices;
        public GameObject weaponMenu;
        //holds the icons for chosen weapons
        public Image[] icons;
        private Weapon[] weaponSelections = new Weapon[2];

        private int _currentSlot = 0;

        [Header("ShieldMenu - Not Implemented Yet")]
        public GameObject shieldMenu;
        public Sprite shieldSelectIcon;

        private bool _isOpened = false;

        public void Start() {
            //unncesscary now but keeping for posterity
            Array.Sort(_weapons, delegate (Weapon w1, Weapon w2) { return w1.ReferenceID.CompareTo(w2.ReferenceID); }); //sort the array by reference IDs

            //assign the button groups the weapon data and such
            for (int i = 0; i < _weaponChoices.Length; i++) {
                _weaponChoices[i].GetComponent<Image>().sprite = _weapons[i].weaponIcon;
                int temp = i;
                _weaponChoices[i].GetComponent<Button>().onClick.AddListener(delegate { SelectWeapon(temp); });
            }
        }

        /// <summary>
        /// assign the weapon as long as it isn't already selected
        /// </summary>
        /// <param name="wep"></param>
        public void SelectWeapon(int wepIndex) {
            if (weaponSelections[0] != _weapons[wepIndex] && weaponSelections[1] != _weapons[wepIndex]) {
                icons[_currentSlot].GetComponent<Image>().sprite = _weapons[wepIndex].weaponIcon;
                weaponSelections[_currentSlot] = _weapons[wepIndex]; 
            }
        }

        /// <summary>
        /// On Opening the machine, set the selected weapons to what the player has already
        /// </summary>
        public void OpenMachine() {
            _isOpened = true;
            GameController.Controller.PlayerReference.SetPlayerFrozen(true);
            weaponMenu.SetActive(true);
            weaponSelections[0] = GameController.Controller.PlayerReference.WeaponsInHand[0];
            weaponSelections[1] = GameController.Controller.PlayerReference.WeaponsInHand[1];
        }

        public void SwapSlots(int newSlot) {
            _currentSlot = newSlot;
        }

        /// <summary>
        /// Toggle the two menus, do nothing for now
        /// </summary>
        public void ToggleMenu() {
            return;
            weaponMenu.SetActive(!weaponMenu.activeSelf);
            shieldMenu.SetActive(!shieldMenu.activeSelf);
        }

        /// <summary>
        /// Apply the chosen to the player
        /// </summary>
        public void Confirm() {
            if (weaponSelections[0] != GameController.Controller.PlayerReference.WeaponsInHand[0] && weaponSelections[1] != GameController.Controller.PlayerReference.WeaponsInHand[1]) {
                Weapon w1 = Instantiate(weaponSelections[0]);
                Weapon w2 = Instantiate(weaponSelections[1]);
                GameController.Controller.PlayerReference.Equip(w1, 0);
                GameController.Controller.PlayerReference.Equip(w2, 1);
                CloseMachine();
            }
        }

        /// <summary>
        /// Turn off menus and give control back to player
        /// </summary>
        public void CloseMachine() {
            weaponMenu.SetActive(false);
            GameController.Controller.PlayerReference.SetPlayerFrozen(false);
            _isOpened = false;
        }
                private void OnTriggerEnter(Collider other) {
            if (other.tag.Equals("Player")) {
                //_interactionkeyToggle.Close();
            }
        }

        private void OnTriggerStay(Collider other) {
            if (!_isOpened && other.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E)) {
                OpenMachine();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.tag.Equals("Player")) {
                //_interactionkeyToggle.Open();
            }
        }
    }
}
