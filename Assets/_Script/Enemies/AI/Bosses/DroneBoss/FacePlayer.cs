using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using MEC;

public class FacePlayer : MonoBehaviour {
    private Transform player;

	// Use this for initialization
	void OnEnable () {
        if (GameController.Controller.PlayerReference == null) {
            Timing.RunCoroutine(DelayedPlayerReferenceGrab());
        }
        else {
            player = GameController.Controller.PlayerReference.transform;
        }
    }

    private IEnumerator<float> DelayedPlayerReferenceGrab() {
        yield return Timing.WaitForSeconds(0.5f);
        if (!GameController.Controller.PlayerReference.IsDead) {
            if (GameController.Controller.PlayerReference.gameObject) {
                player = GameController.Controller.PlayerReference.transform;
            }
        }
        yield return 0f;
    }

    // Update is called once per frame
    void Update () {
        transform.LookAt(player);
	}
}
