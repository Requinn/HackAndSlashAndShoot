using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject.Weapons{
    public class Gun : Weapon{
        public ObjectPoolItem bullet;
        //does this do anything
        public GunClass GunType;
        public Damage.Faction faction;

        public enum GunClass{
            single,
            burst,
            auto
        }

        public override void Fire(){
            throw new System.NotImplementedException();
        }

        public override void ChargeAttack(){
            throw new System.NotImplementedException();
        }
    }
}
