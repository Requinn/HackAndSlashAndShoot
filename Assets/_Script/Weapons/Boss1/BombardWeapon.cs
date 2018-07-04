using System.Collections;
using System.Collections.Generic;
using JLProject.Weapons;
using MEC;
using UnityEngine;

namespace JLProject{
    public class BombardWeapon : Weapon{
        public Explosive bomb;
        public int bombCount; //how many bombs?
        public float bombRadius; //in how wide of an area around the player?
        public float delay; //how long do we delay placement by?
        private GameObject _player;

        void Start(){
            _player = FindObjectOfType<PlayerController>().gameObject;
        }

        public override void Fire(){
            if (delay > 0f){
                Timing.RunCoroutine(DelayedAttack()); //uh??????????????????? why the fuck is this existing?
            }
            else{
                for (int i = 0; i < bombCount; i++){
                    Vector2 rand = Random.insideUnitCircle * bombRadius;
                    //some height issues
                    Instantiate(bomb, _player.transform.position + new Vector3(rand.x, _player.transform.position.y - 1f, rand.y), Quaternion.identity)
                        .Fire();
                }
            }
        }

        private IEnumerator<float> DelayedAttack(){
            yield return Timing.WaitForSeconds(delay);
            for (int i = 0; i < bombCount; i++) {
                Vector2 rand = Random.insideUnitCircle * bombRadius;
                Instantiate(bomb, _player.transform.position + new Vector3(rand.x, _player.transform.position.y - 1f, rand.y), Quaternion.identity)
                    .Fire();
            }
        }
    }
}
