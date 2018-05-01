using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// use this to check for nearby enemies in the medic enemy
    /// </summary>
    public class DetectNearbyAlliesAction : FsmStateAction{
        public FsmOwnerDefault gameobject;


        public override void Awake(){
        }

        public override void OnEnter(){
            Attack();
            Finish();
        }

        public void Attack(){
        }
    }
}