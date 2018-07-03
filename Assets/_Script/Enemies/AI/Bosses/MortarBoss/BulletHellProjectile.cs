using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

public class BulletHellProjectile : MonoBehaviour{
    public float damageValue;
    public float speed;

    private Damage.DamageEventArgs args;

    void OnTriggerEnter(Collider c){
        if (c.transform.CompareTag("Environment")){
            Destroy(gameObject);
        }
        if (c.transform.CompareTag("Player")){
            PlayerController p = c.GetComponent<PlayerController>();
            if (p){
                p.TakeDamage(this, ref args);
                Destroy(gameObject);
            }
        }
    }

	// Use this for inpitialization
	void Start () {
		args = new Damage.DamageEventArgs(damageValue, transform.position, Damage.DamageType.Ranged, Damage.Faction.Enemy);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
