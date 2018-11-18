using System;
using System.Collections;
using System.Collections.Generic;
using DG;
using DG.Tweening;
using UnityEngine;

namespace JLProject{
    public class TranslateObject : Toggleable{
        public Vector3 doorMovement = new Vector3(0f, 6f, 0f);
        public bool StartOpened = false;
        void Awake(){
            if (StartOpened) {
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
            if(Opened) { return; }
            //Debug.Log(transform.position.y - verticalMovement);
            transform.DOMove(transform.position - doorMovement, 0.1f);
            Opened = true;
        }

        public override void Close() {
            if (!Opened) { return; }
            //Debug.Log(transform.position.y + verticalMovement);
            transform.DOMove(transform.position + doorMovement, 0.1f);
            Opened = false;
        }
    }
}
