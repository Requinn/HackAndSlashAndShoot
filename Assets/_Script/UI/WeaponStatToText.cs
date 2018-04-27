using System.Collections;
using System.Collections.Generic;
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
                        + weaponToShow.StatsList[0] + " : " + weaponToShow.AttackValue + "\n"
                        + weaponToShow.StatsList[1] + " : " + weaponToShow.AttackDelay + "\n"
                        + weaponToShow.StatsList[2] + " : " + weaponToShow.ReloadSpeed + "\n"
                        + weaponToShow.StatsList[3] + " : " + weaponToShow.MaxMag + "\n";
        }
    }
}
