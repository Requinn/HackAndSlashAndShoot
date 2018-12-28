using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// spawns an item above this point every X seconds
    /// </summary>
    public class ItemSpawner : MonoBehaviour{
        public float SpawnTime = 3.0f;
        public GameObject ObjectToSpawn;
        private float _timeSinceLastCheck;

        private Ray _upRay;
        private GameObject _spawnedObject = null;
        private RaycastHit _hitData;

        void Start(){
            //put ray origin below object so raycast has a chance to collide with player
            _upRay = new Ray(transform.position - new Vector3(0f,2f,0f), Vector3.up);
        }

        // Update is called once per frame
        void Update(){
            if (_spawnedObject == null && _timeSinceLastCheck < SpawnTime){
                _timeSinceLastCheck += Time.deltaTime;
            }

            if (_timeSinceLastCheck >= SpawnTime){
                _timeSinceLastCheck = 0;
                Debug.DrawRay(_upRay.origin, _upRay.direction, Color.red, 150f);
                Debug.Log(_upRay.origin);
                if (_spawnedObject == null && !Physics.SphereCast(_upRay, 1f, 5f, ~(1 << LayerMask.NameToLayer("Environment")))){
                    _spawnedObject = Instantiate(ObjectToSpawn, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
                }
            }
        }
    }
}