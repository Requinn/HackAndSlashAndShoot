using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using UnityEditor;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// interface to handle weapon mods
    /// </summary>
    public interface IWeaponModifier{
        string ModName { get; set; }
        ModType modType{ get; set; }
        string[] ValidWeapon { get; set; }
        Dictionary<string, float> ModifiedStats{ get; set; }
        void ApplyMod(Weapon w);
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
