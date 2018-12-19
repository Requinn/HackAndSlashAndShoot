using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

public class HealPlayerOnLoad : MonoBehaviour {
    private void Start() {
        Invoke("HealPlayer", 0.25f);
    }

    private void HealPlayer() {
        GameController.Controller.PlayerReference.Heal(gameObject, 999);
    }
}
