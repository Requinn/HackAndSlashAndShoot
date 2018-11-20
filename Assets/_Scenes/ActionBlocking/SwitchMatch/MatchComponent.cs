using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class used by MatchCheckObjective to define things that can send a check signal to the objective
/// </summary>
public abstract class MatchComponent : MonoBehaviour {
    //we hit a correct switch
    protected Action _OnMatch = delegate { };
    public Action OnMatch {
        get { return _OnMatch; }
        set { _OnMatch = value; }
    }

    //we hit the wrong switch
    protected Action _WrongMatch = delegate { };
    public Action WrongMatch {
        get { return _WrongMatch; }
        set { _WrongMatch = value; }
    }

    /// <summary>
    /// Is this object matched?
    /// </summary>
    protected bool _isMatched;
    public bool IsMatched {
        get { return _isMatched; }
    }
}