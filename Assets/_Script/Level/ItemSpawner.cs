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

        void Start(){
            _upRay = new Ray(transform.position, Vector3.up);
        }

        // Update is called once per frame
        void Update(){
            if (_spawnedObject == null && _timeSinceLastCheck < SpawnTime){
                _timeSinceLastCheck += Time.deltaTime;
            }

            if (_timeSinceLastCheck >= SpawnTime){
                _timeSinceLastCheck = 0;
                if (_spawnedObject == null && !Physics.Raycast(_upRay, 5f)){
                    _spawnedObject = Instantiate(ObjectToSpawn, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
                }
            }
        }
    }
}