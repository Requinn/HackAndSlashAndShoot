using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

/// <summary>
/// test script to check for player taking damage
/// </summary>
public class EventTestScript : MonoBehaviour {
    private void Awake(){
        PlayerController p = FindObjectOfType<PlayerController>();
        p.TookDamage += HandlePlayerDamaged;
        p.HealDamage += HandlePlayerHealed;

        TestDummy t = FindObjectOfType<TestDummy>();
        t.TookDamage += HandlePlayerDamaged;
        t.OnDeath += HandleDeath;
    }

    private void HandleDeath(){
        Debug.Log("You killed it");
    }
    private void HandlePlayerHealed(float hp){
        Debug.Log("+"+hp);
    }

    private void HandlePlayerDamaged(float hp){
        Debug.Log("-" + hp);
    }
}
