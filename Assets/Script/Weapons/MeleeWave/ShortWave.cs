using System.Collections.Generic;
using MEC;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace JLProject{
    public class ShortWave : Melee{
        public GameObject WaveComponent;
        public CollisionReport CR;
        public float Damage = 15.0f;
        void Start(){
            CurMag = MaxMag = 3;
            AttackDelay = 0.25f;
            ReloadSpeed = 0.65f;
            WaveComponent.GetComponent<WaveCollision>().args = new Damage.DamageEventArgs(Damage, this.transform.position, JLProject.Damage.DamageType.Melee, JLProject.Damage.Faction.Player);
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

