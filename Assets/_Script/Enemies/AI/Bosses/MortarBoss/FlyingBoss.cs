using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JLProject;
using MEC;
using UnityEngine;

public class FlyingBoss : AIEntity{
    public Transform[] FlightPositions;
    public Transform[] DownedPositions;
    public float DownStateDuration = 10f;

    private bool _isDowned;
    private int _currentNodeIndex = 0;

	// Use this for initialization
	void Start () {
		
	}

    public void Initiate(){
        
    }

    // Update is called once per frame
	void Update () {
		if(!_isDowned) Movement();
	}

    /// <summary>
    /// Called by the mortar
    /// </summary>
    public void GetHit(){
        Debug.Log("AAA");
        _isDowned = true;
        var damageEventArgs = new Damage.DamageEventArgs(150f, transform.position, Damage.DamageType.Neutral, Damage.Faction.Player);
        TakeDamage(this, ref damageEventArgs);
        Timing.RunCoroutine(DownedCo());
    }

    /// <summary>
    /// Called when struck, moving to a downed position
    /// </summary>
    private void MoveToDownedNode(){
        DOTween.Kill("Movement");
        transform.DOMove(DownedPositions[_currentNodeIndex].position, 0.75f).SetEase(Ease.Linear);
        _currentNodeIndex++;
        if (_currentNodeIndex > 3) {
            _currentNodeIndex = 0;
        }
    }

    /// <summary>
    /// Called for normal movements
    /// </summary>
    private void MoveToNextNode(){
        transform.DOMove(FlightPositions[_currentNodeIndex].position, 2f).SetId("Movement").SetEase(Ease.Linear);
    }

    private IEnumerator<float> DownedCo(){
        MoveToDownedNode();
        yield return Timing.WaitForSeconds(DownStateDuration);
        _isDowned = false;
    }

    protected override void Movement(){
        if (!V3Equal(transform.position, FlightPositions[_currentNodeIndex].position)){
            MoveToNextNode();
        }
        if (V3Equal(transform.position, FlightPositions[_currentNodeIndex].position)) {
            _currentNodeIndex++;
            if (_currentNodeIndex > 3) {
                _currentNodeIndex = 0;
            }
        }
    }

    private bool V3Equal(Vector3 a, Vector3 b) {
        return Vector3.SqrMagnitude(a - b) < 2;
    }

    protected override void Attack(){
        throw new System.NotImplementedException();
    }

    public override void ProjectileResponse(){
        throw new System.NotImplementedException();
    }

}
