using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public interface IProjectile{
        float GetVelocity();
        void SetFaction(Damage.Faction f);
    }
}

