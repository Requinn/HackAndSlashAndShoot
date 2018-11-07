using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;
using System;
using MEC;
using UnityEngine.AI;

public class Yanker : MeleeUnit {
    public YankerPull pullAbility;

    protected new void Start() {
        base.Start();
        Timing.RunCoroutine(AICycle());
    }

    protected override void Movement() {
        if (_vision.inRange) {
            base.Rotate();
            if (pullAbility.CanCast && Vector3.Distance(transform.position, target.transform.position) > pullAbility.PullRange) {
                //if (AnimController) AnimController.WalkForward();
                _NMAgent.Move(transform.forward * speed * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, target.transform.position) > _vision.maxAttackRange) {
                //if (AnimController) AnimController.WalkForward();
                _NMAgent.Move(transform.forward * speed * Time.deltaTime);
            }
        }
        else {
            //if (AnimController) AnimController.Idle();
        }
    }

    private IEnumerator<float> AICycle() {
        //while we are alive
        while (CurrentHealth > 0.0f) {
            if(!target) { yield return 0; }
            _vision.CanSeeTarget(target.transform);
            //if we can see the player
            if (_vision.inRange) {
                //and our ability is ready and in range
                if (pullAbility.CanCast && pullAbility.InRange) {
                    //wait a little
                    yield return Timing.WaitForSeconds(0.25f);
                    //if the player is still in range
                    if (pullAbility.InRange) {
                        pullAbility.CastPull();
                        yield return Timing.WaitForSeconds(0.1f);
                    }else { //else move
                        Movement();
                    }
                }
                //and they are in the attack range
                else if (_vision.inAttackCone) {
                    weapon.Fire();
                }
                //move
                else {
                    Movement();
                }
            }
            else {
                Movement();
            }
            yield return 0f;
        }

    }
}
