using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using JLProject;
using JLProject.Weapons;
using UnityEngine;

public class DamageMod2 : WeaponModifier{
    [SerializeField] private string _name = "Damage Mod";
    [SerializeField] private ModType _modType = ModType.GunReciever;
    public bool _applied = false;
    private string[] _applicableWeapons = {"ShortPitol", "BurstGun"};

    private Dictionary<string, float> _modifiedStats =
        new Dictionary<string, float>(){{"Damage", 15f},{"AttackDelay", 1.5f}};

    void Start(){
        ModName = _name;
        modType = _modType;
        Applied = _applied = false;
        ValidWeapon = _applicableWeapons;
        ModifiedStats = _modifiedStats;
    }

    //I don't like this
    public override void ApplyMod(Weapon w){
        if (!Applied){
            w.AttackValue += _modifiedStats["Damage"];
            w.AttackDelay += _modifiedStats["AttackDelay"];
            Applied = true;
        }
    }

    public override void RemoveMod(Weapon w){
        if (Applied){
            w.AttackValue -= _modifiedStats["Damage"];
            w.AttackDelay -= _modifiedStats["AttackDelay"];
            Applied = false;
        }
    }
}
