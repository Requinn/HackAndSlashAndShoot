using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject {
    /// <summary>
    /// Weapon Modification handler, attached to a weapon you wish to modify.
    /// Currently, mods work
    /// TODO: BE CAUTIOS OF DELETING MODS WHILE NOT PLAYING
    /// </summary>
    public class WeaponsModHandler : MonoBehaviour{
        public List<WeaponModifier> _weaponMods = new List<WeaponModifier>();
        public Weapon weapon;

        //modify the stats of a weapon
        public void ModifyStats() {
            foreach (var mod in _weaponMods){
                mod.ApplyMod(weapon);
            }
        }

        //try to add a weapon mod
        public void AddMod(WeaponModifier mod){
            bool contains = false;
            foreach (var m in _weaponMods){
                if (m.modType == mod.modType){
                    contains = true;
                    break;
                }
            }
            if (!contains){
                _weaponMods.Add(mod);
                ModifyStats();
            }
        }

        public void RemoveMod(WeaponModifier mod){
            if (_weaponMods.Contains(mod)){
                mod.RemoveMod(weapon);
                _weaponMods.Remove(mod);
            }
        }

        public void RemoveModByType(ModType type){
            foreach (var mod in _weaponMods){
                if (mod.modType == type){
                    RemoveMod(mod);
                }
            }
        }

        //remove mod1 and replace it with mod2
        public bool SwapMod(WeaponModifier mod1, WeaponModifier mod2){
            //swap a mod
            if (mod1.modType == mod2.modType){
                RemoveMod(mod1);
                AddMod(mod2);
                return true;
            }
            return false;
        }

        public void ClearMods(){
            _weaponMods.Clear();
        }
    }
}
