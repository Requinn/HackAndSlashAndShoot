using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds information on what this receiver gets, and responds accordingly.
/// </summary>
public class ThrownObjectReceiver : MatchComponent {
    [SerializeField]
    private MatchCheckObjective.MatchCases _matchType;
    public MatchCheckObjective.MatchCases MatchType {
        get { return _matchType; }
    }

    public void DoMatch() {
        gameObject.SetActive(false);
        _isMatched = true;
        OnMatch();
    }
}
