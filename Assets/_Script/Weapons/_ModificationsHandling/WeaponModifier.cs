using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// interface to handle weapon mods
    /// </summary>
    public abstract class WeaponModifier : MonoBehaviour{
        public string ModName { get; set; }
        public bool Applied{ get; set; }
        public ModType modType{ get; set; }
        public string[] ValidWeapon { get; set; }
        public Dictionary<string, float> ModifiedStats{ get; set; }
        public abstract void ApplyMod(Weapon w);
        public abstract void RemoveMod(Weapon w);
    }

    public enum ModType {
        GunReciever,
        GunMagazine,
        GunProjectile,
        GunModule,
        
        SwordBlade,
        SwordGrip,
        SwordModule,

        ShieldFrame,
        ShieldModule
    }
}
