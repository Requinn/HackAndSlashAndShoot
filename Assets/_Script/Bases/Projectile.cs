using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public interface IProjectile{
        void SetDamage(float damage);
        float GetVelocity();
        void ResetLife();
        void SetFaction(Damage.Faction f);
        Damage.Faction GetFaction();
    }
}

