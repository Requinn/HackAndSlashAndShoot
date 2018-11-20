using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

public class MatchCheckObjective : LevelObjective {
    [SerializeField]
    private bool _isDynamicChecking = true; //should we check all the time?
    [SerializeField]
    private MatchComponent[] _matchedObjects;
    private bool[] _matchedFlags;

    public enum MatchCases {
        A, B, C, D
    }

    // Use this for initialization
    void Start() {
        //initialize the flag array, as well as having every reciever update our solution
        _matchedFlags = new bool[_matchedObjects.Length];
        //if we are dynamic checking, then subscribe to the OnMatch event to check every time we match something
        if (_isDynamicChecking) {
            for (int i = 0; i < _matchedObjects.Length; i++) {
                _matchedObjects[i].OnMatch += CheckSolution;
            }
        }
    }

    /// <summary>
    /// Check if everything we have matches
    /// </summary>
    public void CheckSolution() {
        //check if everything is matched, leaving if one thing isn't matched
        foreach(var m in _matchedObjects) {
            if (!m.IsMatched) {
                return;
            }
        }
        //we finished
        onCompleteObjective();
        ObjectToUnlock.Open();
    }
}
