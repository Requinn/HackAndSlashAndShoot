using System;
using System.Collections;
using System.Collections.Generic;
using DG;
using DG.Tweening;
using UnityEngine;

namespace JLProject{
    public class VerticalDoor : Toggleable{
        public bool Locked = false;
        public bool Opened = false;
        public float verticalMovement = 6.0f;
        void Awake(){
            if (Opened){
                Open();
            }
        }

        public override void Toggle(){
            if (Opened){
                Close();
            }
            else if (!Opened){
                Open();
            }
        }

        public override void Open(){
            //Debug.Log(transform.position.y - verticalMovement);
            transform.DOMoveY(transform.position.y - verticalMovement, 0.1f);
            Opened = true;
        }

        public override void Close() {
            //Debug.Log(transform.position.y + verticalMovement);
            transform.DOMoveY(transform.position.y + verticalMovement, 0.1f);
            Opened = false;
        }
    }
}
