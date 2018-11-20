using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Thrown object to interact with a switch to match with.
/// </summary>
public class ThrownObjectIdentifier : MonoBehaviour {
    [SerializeField]
    private MatchCheckObjective.MatchCases _matchType;

    private void OnTriggerEnter(Collider other) {
        ThrownObjectReceiver rcvr = other.GetComponent<ThrownObjectReceiver>();
        //if other object is of type thrown receiver
        if (rcvr) {
            //and they are our type
            if (_matchType == rcvr.MatchType) {
                rcvr.DoMatch(); //do the match
            }
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Environment" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Switch") {
            Destroy(gameObject);
        }
    }
}
