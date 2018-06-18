using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JLProject.Weapons;
using UnityEngine;

namespace JLProject{
    public class ModMenu : MonoBehaviour{
        public PlayerController pc;
        /// <summary>
        /// apply the new stats to the weapon in hand as well.
        /// </summary>
        /// <param name="w"></param>
        public void ConfirmMods(Weapon w){
            for (int i = 0; i < pc.WeaponsInHand.Count; i++){
                if (pc.WeaponsInHand[i].ReferenceID == w.ReferenceID){
                    pc.WeaponsInHand[i].AttackValue = w.AttackValue;
                    pc.WeaponsInHand[i].AttackDelay = w.AttackDelay;
                    pc.WeaponsInHand[i].CurMag = w.MaxMag;
                    pc.WeaponsInHand[i].MaxMag = w.MaxMag;
                    pc.WeaponsInHand[i].ReloadSpeed = w.ReloadSpeed;
                }
            }
        }
    }
}
