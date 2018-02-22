using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public interface IDamageable{
        Damage.Faction GetFaction();
        void ApplyDamage(object sender, ref Damage.DamageEventArgs args);
    }
}
