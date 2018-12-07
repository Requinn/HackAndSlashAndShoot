using HutongGames.PlayMaker;
using JLProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YankerPull : MonoBehaviour {
    [SerializeField]
    private float _PullRange = 8f;
    [SerializeField]
    private float _pullAngle = 90f;
    [SerializeField]
    private float _pullStrength = 80f;
    [SerializeField]
    private float _coolDown = 15f;

    private float _timeSinceCast = 15f;
    private bool _isReadytoCast = true;
    private bool _isInRange = false;

    public bool CanCast { get { return _isReadytoCast; } }
    public bool InRange { get { return _isInRange; } }
    public float PullRange { get { return _PullRange; } }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.Controller.PlayerReference) {
            _isInRange = false;
            CheckReady();
            if (_isReadytoCast) {
                CheckRange();
                if (_isInRange) {
                    //CastPull();
                }
            }
        }
        
	}

    /// <summary>
    /// Add a force to the player towards this location at pull force if they can move
    /// </summary>
    public void CastPull() {
        ImpactReceiver playerImpact = GameController.Controller.PlayerReference.GetComponent<ImpactReceiver>();
        if (playerImpact.isActiveAndEnabled) {
            playerImpact.GetComponent<ImpactReceiver>().AddImpact(transform.position - GameController.Controller.PlayerReference.transform.position, _pullStrength);
        }
        _timeSinceCast = 0.0f;
    }

    /// <summary>
    /// Check if we're on cooldown
    /// </summary>
    public void CheckReady() {
        if(_timeSinceCast >= _coolDown) {
            _isReadytoCast = true;
        }else {
            _timeSinceCast += Time.deltaTime;
            _isReadytoCast = false;
        }
    }

    /// <summary>
    /// Simplified FoVDetection to check if this ability is ready to cast
    /// </summary>
    public void CheckRange() {
        float distSQ = (GameController.Controller.PlayerReference.transform.position - transform.position).sqrMagnitude;
        //check for general range
        if(distSQ < (_PullRange * _PullRange)) {
            //Then check for angle relative to our forward
            Vector3 relativePos = transform.InverseTransformPoint(GameController.Controller.PlayerReference.transform.position);
            Vector3 projected = Vector3.Project(relativePos, Vector3.forward);
            if (projected.sqrMagnitude > 0.0f) {
                float angle = Vector3.Angle(GameController.Controller.PlayerReference.transform.position - transform.position, transform.forward);
                if(angle <= _pullAngle) {
                    _isInRange = true;
                }
            }
        }
    }
}

public class YankerAbilityCheck : FsmStateAction {
    public FsmBool pullReady = false;
    public FsmBool pullInRange = false;
    public FsmOwnerDefault gameObject;
    private YankerPull pull;

    public override void OnEnter() {
        pull = Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<YankerPull>();
        pullReady.Value = pull.CanCast;
        pullInRange.Value = pull.InRange;
        Finish();
    }
}

public class YankerAbilityAction : FsmStateAction {
    public FsmOwnerDefault gameObject;
    private YankerPull pull;

    public override void OnEnter() {
        pull = Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<YankerPull>();
        pull.CastPull();
        Finish();
    }
}
