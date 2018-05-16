using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// spawns an item above this point every X seconds
    /// </summary>
    public class ItemSpawner : MonoBehaviour{
        public float SpawnTime = 5.0f;
        public GameObject ObjectToSpawn;
        private float _timeSinceLastCheck;

        private Ray _upRay;
        private RaycastHit _upRayHit;

        void Start(){
            _upRay = new Ray(transform.position, Vector3.up);
        }

        // Update is called once per frame
        void Update(){
            if (_timeSinceLastCheck < SpawnTime){
                _timeSinceLastCheck += Time.deltaTime;
            }

            if (_timeSinceLastCheck >= SpawnTime){
                _timeSinceLastCheck = 0;
                if (!Physics.Raycast(_upRay, out _upRayHit, 2f)){
                    Instantiate(ObjectToSpawn, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
                }
            }
        }
    }
}