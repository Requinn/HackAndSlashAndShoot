using JLProject;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private bool _contact = false;

    void Update() {
        if (_contact && Input.GetKeyDown(KeyCode.E)) {
        }
    }

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.CompareTag("Player")) {
            _contact = true;
        }
    }

    void OnTriggerExit(Collider c) {
        if (c.gameObject.CompareTag("Player")) {
            _contact = false;
        }
    }
}
