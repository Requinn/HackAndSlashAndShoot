using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

public class Melee : Weapon {
    public float attackDelay = 0.65f;
    public float comboDelay = 0.75f;
    public int swingCount = 3;

    [SerializeField]
    protected int _currentCombo = 0;
    protected float _timeSinceSwing = 0.0f;
    protected float _comboDecay = 0.7f;

    public int CurrentCombo {
        get {
            return _currentCombo;
        }

        set {
            _currentCombo = value;
        }
    }

    public override void Fire(){
        throw new System.NotImplementedException();
    }
}
