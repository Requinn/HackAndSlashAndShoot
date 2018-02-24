using System;
using System.Collections;
using System.Collections.Generic;
using DG;
using DG.Tweening;
using UnityEngine;

namespace JLProject{
    public class VerticalDoor : Unlockable{
        public bool Locked = false;
        public bool Opened = false;

        void Awake(){
            if (Opened){
                Open();
            }
        }

        public override void Open(){
            transform.DOMoveY(transform.position.y - 6.0f, 0.1f, true);
        }

        public override void Close() {
            transform.DOMoveY(transform.position.y + 6.0f, 0.1f, true);
        }
    }
}
