using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public class BreakableCube : BreakableObject{
        public bool isBreakable = true;

        public override void Hit(){
            if (isBreakable){
                base.Hit();
            }
        }
    }
}
