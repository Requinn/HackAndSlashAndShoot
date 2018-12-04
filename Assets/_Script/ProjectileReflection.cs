using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

public class ProjectileReflection : MonoBehaviour {
    private void OnTriggerEnter(Collider collision) {{
            var bullet = collision.gameObject.GetComponent<IProjectile>();
            if (bullet != null && bullet.GetFaction() == Damage.Faction.Player) {
                var bulletPhys = collision.GetComponent<Rigidbody>();
                bullet.ResetLife();
                bullet.SetFaction(Damage.Faction.Enemy);
                bulletPhys.velocity = -bulletPhys.velocity;
            }
        }
    }
}
