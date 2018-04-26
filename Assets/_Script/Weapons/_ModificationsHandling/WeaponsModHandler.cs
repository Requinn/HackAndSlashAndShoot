using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject {
    /// <summary>
    /// and interface applied to weapons that will handle their modifications
    /// </summary>
    public class WeaponsModHandler : MonoBehaviour{
        private List<IWeaponModifier> _weaponMods = new List<IWeaponModifier>();

        //modify the stats of a weapon
        public void ModifyStats(Weapon w) {
            foreach (var mod in _weaponMods){
                foreach (var appplicablestat in mod.ModifiedStats){
                    if (!w.StatsList.Contains(appplicablestat.Key)){
                        break;
                    }
                    mod.ApplyMod(w);
                }
            }
        }

        //try to add a weapon mod
        public bool AddMod(IWeaponModifier mod){
            foreach (var m in _weaponMods){
                if (m.modType == mod.modType){
                    return false;
                }
            }
            Debug.Log("aa");
            _weaponMods.Add(mod);
            return true;
        }

        public void RemoveMod(IWeaponModifier mod){
            _weaponMods.Remove(mod);
        }

        //remove mod1 and replace it with mod2
        public bool SwapMod(IWeaponModifier mod1, IWeaponModifier mod2){
            //swap a mod
            if (mod1.modType == mod2.modType){
                RemoveMod(mod1);
                AddMod(mod2);
                return true;
            }
            return false;
        }
    }
}
