using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using UnityEngine;

namespace JLProject{
    public static partial class Damage{
        [System.Serializable]
        public struct DamageEventArgs{
            public float DamageValue;
            public DamageType DamageType;
            public Vector3 HitPoint;
            public Faction SourceFaction;

            public DamageEventArgs(float damageValue, Vector3 hitPoint, DamageType type = DamageType.Neutral, Faction faction = Faction.Neutral){
                DamageValue = damageValue;
                DamageType = type;
                HitPoint = hitPoint;
                SourceFaction = faction;
            }
        }
    }
}
