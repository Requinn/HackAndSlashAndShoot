using System.Collections;
using System.Collections.Generic;
using JLProject.Weapons;
using MEC;
using UnityEngine;

namespace JLProject{
    public class BombardWeapon : Weapon{
        public Explosive bomb;
        public int bombCount;
        public float bombRadius;
        public float delay;
        private GameObject _player;

        void Start(){
            _player = FindObjectOfType<PlayerController>().gameObject;
        }
        public override void Fire(){
            if (delay > 0f){
                Timing.RunCoroutine(DelayedAttack());
            }
            else{
                for (int i = 0; i < bombCount; i++){
                    Vector2 rand = Random.insideUnitCircle * bombRadius;
                    Instantiate(bomb, _player.transform.position + new Vector3(rand.x, 0, rand.y), Quaternion.identity)
                        .Fire();
                }
            }
        }

        private IEnumerator<float> DelayedAttack(){
            yield return Timing.WaitForSeconds(delay);
            for (int i = 0; i < bombCount; i++) {
                Vector2 rand = Random.insideUnitCircle * bombRadius;
                Instantiate(bomb, _player.transform.position + new Vector3(rand.x, 0, rand.y), Quaternion.identity)
                    .Fire();
            }
        }
    }
}
