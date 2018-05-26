using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public class MortarFire : Toggleable{
        public Transform MortarBarrel;
        public float HitTimer = 20f;
        private float _currentTime = 0f;
        private bool _readyToFire;
        private Ray _fwdRay;
        private RaycastHit _fwdHit;
        // Use this for initialization
        void Start(){
            _fwdRay.origin = MortarBarrel.position;
            _fwdRay.direction = MortarBarrel.forward;
        }

        IEnumerator<float> TimerCoroutine(float time){
            while (_currentTime < HitTimer){
                _currentTime += Time.deltaTime;
                yield return 0f;
            }
            _readyToFire = true;
            yield return 0f;
        }

        // Update is called once per frame
        void Update(){
        }

        public override void Toggle(){
            FireMortar();
        }

        void FireMortar(){
            if (Physics.Raycast(_fwdRay, out _fwdHit, 25f)){
                if (_fwdHit.transform.CompareTag("Enemy")){
                    _fwdHit.transform.GetComponent<FlyingBoss>().GetHit();
                }
            }
        }

        public override void Open(){
            throw new System.NotImplementedException();
        }

        public override void Close(){
            throw new System.NotImplementedException();
        }
    }
}
