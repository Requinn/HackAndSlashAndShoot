using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

public class Gun : Weapon {
    public ObjectPoolItem bullet;
    public GunClass GunType;
    public enum GunClass{
        single,
        burst,
        auto
    }
    public override void Fire(){
        throw new System.NotImplementedException();
    }
}
