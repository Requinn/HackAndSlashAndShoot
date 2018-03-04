using System.Collections.Generic;
using MEC;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace JLProject{
    public class ShortWave : Melee{
        public GameObject WaveComponent;
        public float attackDelay = 0.65f;
        public float comboDelay = 1.25f;
        public int swingCount = 3;
        public Damage.Faction faction;
        public float Damage = 15.0f;
        void Start(){
            CurMag = MaxMag = swingCount;
            AttackDelay = attackDelay;
            ReloadSpeed = comboDelay;
            WaveComponent.GetComponent<WaveCollision>().args = new Damage.DamageEventArgs(Damage, this.transform.position, JLProject.Damage.DamageType.Melee, faction);
        }

        public override void Fire(){
            if (_canAttack){
                WaveComponent.SetActive(true);
                WaveComponent.GetComponent<MeshRenderer>().enabled = true; //This is to re enable the mesh, for some reason it turns off and stays off
                Timing.RunCoroutine(WaveDelay());
                CurMag--;
                if (CurMag == 0){
                    Timing.RunCoroutine(Reload());
                }
                else{
                    Timing.RunCoroutine(Delay());
                }
            }
        }

        private IEnumerator<float> WaveDelay(){
            yield return Timing.WaitForSeconds(0.025f);
            WaveComponent.SetActive(false);
        }
    }

}

