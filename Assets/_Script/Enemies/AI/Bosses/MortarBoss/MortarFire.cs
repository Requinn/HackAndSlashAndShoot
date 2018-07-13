using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace JLProject{
    public class MortarFire : Toggleable{
        public Transform MortarBarrel;
        public float HitTimer = 20f;
        public MortarBoss Boss;
        public GameObject arrowSwitchMarker;
        private float _currentTime = 0f;
        private bool _readyToFire;
        private Ray _fwdRay;
        private RaycastHit _fwdHit;

        private Damage.DamageEventArgs _damageArgs;

        // Use this for initialization
        void Start(){
            Boss.WeakPointExposed += MakeSelfObvious;
            Boss.WeakPointCovered += RevertVisuals;

            _fwdRay.origin = MortarBarrel.position;
            _fwdRay.direction = MortarBarrel.forward;
            _damageArgs = new Damage.DamageEventArgs(500f, transform.position, Damage.DamageType.Explosive, Damage.Faction.Player, 500f);
        }

        /// <summary>
        /// forcibly turn off the visuals
        /// </summary>
        private void RevertVisuals(){
            arrowSwitchMarker.SetActive(false);
        }

        /// <summary>
        /// start the timed coroutine to make self visible as heck
        /// </summary>
        /// <param name="timer"></param>
        private void MakeSelfObvious(float timer){
            Timing.RunCoroutine(ShowVisual(timer));
        }

        /// <summary>
        /// timed coroutine to show visuals
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        private IEnumerator<float> ShowVisual(float timer){
            arrowSwitchMarker.SetActive(true);
            yield return Timing.WaitForSeconds(timer);
            arrowSwitchMarker.SetActive(false);
            yield return 0f;
        }

        /// <summary>
        /// is this not used??
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
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
                    _fwdHit.transform.GetComponent<Entity>().TakeDamage(this, ref _damageArgs);
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
