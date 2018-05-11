using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public static partial class Damage{
        [System.Serializable]
        public struct DamageEventArgs{
            public float DamageValue;
            public DamageType DamageType;
            public Vector3 HitSourceLocation;
            public Faction SourceFaction;
            public float HitForce;

            public DamageEventArgs(float damageValue, Vector3 hitPoint, DamageType type = DamageType.Neutral, Faction faction = Faction.Neutral, float force = 1f){
                DamageValue = damageValue;
                DamageType = type;
                HitSourceLocation = hitPoint;
                SourceFaction = faction;
                HitForce = force;
            }
        }
    }
}
