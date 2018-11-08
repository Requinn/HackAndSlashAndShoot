using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

public class ProjectileReflection : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("intiialized");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision) {{
            Debug.Log(collision.name);
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
