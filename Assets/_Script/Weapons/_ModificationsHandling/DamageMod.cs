using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public class DamageMod : MonoBehaviour, IWeaponModifier{
        private string _name = "Damage Mod";
        private ModType _modType = ModType.GunReciever;
        private string[] _applicableWeapons = { "ShortPitol", "BurstGun" };
        private Dictionary<string, float> _modifiedStats = new Dictionary<string, float>() { {"Damage", 5f} };

        public string ModName{
            get{ return _name; }
            set{ _name = value; }
        }
        public ModType modType{
            get{ return _modType; }
            set{ _modType = value; }
        }
        public string[] ValidWeapon{
            get{ return _applicableWeapons; }
            set{ _applicableWeapons = value; }
        }

        public Dictionary<string, float> ModifiedStats{
            get{ return _modifiedStats; }
            set{ _modifiedStats = value; }
        }
        
        //I don't like this
        public void ApplyMod(Weapon w){
            w.AttackValue += _modifiedStats["Damage"];
        }
    }
}
