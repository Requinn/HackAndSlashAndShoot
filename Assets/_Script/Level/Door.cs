using System;
using System.Collections;
using System.Collections.Generic;
using DG;
using DG.Tweening;
using UnityEngine;

namespace JLProject{
    public class Door : Toggleable{
        public Vector3 doorMovement = new Vector3(0f, 6f, 0f);

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
            transform.DOMove(transform.position - doorMovement, 0.1f);
            Opened = true;
        }

        public override void Close() {
            //Debug.Log(transform.position.y + verticalMovement);
            transform.DOMove(transform.position + doorMovement, 0.1f);
            Opened = false;
        }
    }
}
