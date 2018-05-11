using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace JLProject {
    public class HeavySmasher : MeleeUnit{
        
        protected override void Attack(){
            weapon.Fire();
        }

        public override void ProjectileResponse() {
            throw new NotImplementedException();
        }
    }
}