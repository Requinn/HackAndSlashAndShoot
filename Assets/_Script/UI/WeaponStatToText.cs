using System.Collections;
using System.Collections.Generic;
using JLProject.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace JLProject{
    public class WeaponStatToText : MonoBehaviour{
        public Weapon weaponToShow;
        public Text text;
        
        void OnEnable(){
            UpdateWeaponText();
        }

        public void UpdateWeaponToShow(Weapon w){
            weaponToShow = w;
            UpdateWeaponText();
        }
        public void UpdateWeaponText(){
            text.text = weaponToShow.name + "\n"
                        + "Attack: " + weaponToShow.AttackValue + "\n"
                        + "Rate of Fire: " + weaponToShow.AttackDelay + "\n"
                        + "Reload Speed: " + weaponToShow.ReloadSpeed + "\n"
                        + "Magazine Size: " + weaponToShow.MaxMag + "\n";
        }
    }
}
