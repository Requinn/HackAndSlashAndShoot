using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

namespace JLProject{
    public class PlayerController : Entity{
        // Use this for initialization
        public float speed;
        private float _MovX, _MovZ;
        private CharacterController cc;
        private Vector3 _MoveDir = Vector3.zero; //not going anywhere
        private Vector3 _MousePos;
        private int floorMask;
        public Weapon CurrentWeapon;

        public Transform WeaponAttachPoint;
        public Transform ShieldAttachPoint;

        private List<Weapon> _weaponsInHand = new List<Weapon>(2);

        void Awake(){
            floorMask = LayerMask.GetMask("Floor");
        }
        void Start(){
            MovementSpeed = speed;
            cc = GetComponent<CharacterController>();
            Faction = Damage.Faction.Player;
        }

        // Update is called once per frame
        void Update(){
            if (!GameController.Controller.paused){
                _MousePos = GetMousePosition();
                AngleUpdate(_MousePos);
                Movement();
                MouseInput();
                WeaponSwap();
            }
        }

        /// <summary>
        /// gets mous input
        /// </summary>
        private void MouseInput(){
            if (Input.GetMouseButtonDown(0) && CurrentWeapon != null){               
                CurrentWeapon.Fire();
            }
        }

        /// <summary>
        /// character motor
        /// </summary>
        protected override void Movement(){
            _MovX = Input.GetAxisRaw("Horizontal");
            _MovZ = Input.GetAxisRaw("Vertical");
            _MoveDir = new Vector3(_MovX, 0, _MovZ).normalized;
            _MoveDir *= MovementSpeed;
            cc.Move(_MoveDir * Time.deltaTime);
        }

        /// <summary>
        /// rotates the characters towards the mouse
        /// TODO fix bug where rotations are off because the camera is angled and the mous inpout doesn't reflect that
        /// </summary>
        private Vector3 GetMousePosition(){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //cast a ray to the mouse position in world
            Plane hPlane = new Plane(Vector3.up, Vector3.zero);             //create a plane using the up vector and origin
            float dist = 0.0f;
            if (hPlane.Raycast(ray, out dist)){
                //OUT where our mouse sits on this plane and raycast the direction
                return ray.GetPoint(dist);
            }
            return Vector3.zero;
        }

        /// <summary>
        /// updates our rotation
        /// </summary>
        /// <param name="tpos"></param>
        private void AngleUpdate(Vector3 tpos){
            Vector3 pos = transform.position;
            transform.rotation = Quaternion.Euler(0, -GetAngle(pos, tpos) * 180.0f / Mathf.PI, 0);  //rotate towards an angle created by our position and direction
        }

        /// <summary>
        /// custom get angle to get the angle using the X and Z components
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float GetAngle(Vector3 v1, Vector3 v2) {
            return Mathf.Atan2(v2.z - v1.z, v2.x - v1.x);
        }

        /// <summary>
        /// Fires weapon
        /// </summary>
        protected override void Attack(){
            CurrentWeapon.Fire();
        }

        /// <summary>
        /// Equips a GO to the attachpoint
        /// </summary>
        /// <param name="GO"></param>
        public void Equip(GameObject GO){
            if (GO.GetComponent<Weapon>()){
                //we have a weapon in both slots, replace the current by:
                //setting current weapon to the new instance, destroying the old one, then setting our current weapon to the one we just picked up
                if (_weaponsInHand.Count >= 2){
                    /**if (CurrentWeapon.type == Weapon.Type.Ranged){
                        ObjectPooler.ObjectPool.UnPoolItem(CurrentWeapon.GetComponent<Gun>().bullet);
                    }**/
                    _weaponsInHand[_weaponsInHand.IndexOf(CurrentWeapon)] = GO.GetComponent<Weapon>();
                    Destroy(CurrentWeapon.gameObject);
                    CurrentWeapon = GO.GetComponent<Weapon>();
                    GO.transform.parent = WeaponAttachPoint.transform;
                    GO.transform.position = WeaponAttachPoint.position;
                    GO.transform.rotation = WeaponAttachPoint.rotation;
                }
                else{
                    if (CurrentWeapon == null){
                        //if we don't have one
                        _weaponsInHand.Add(GO.GetComponent<Weapon>());
                        CurrentWeapon = _weaponsInHand[0];
                        GO.transform.parent = WeaponAttachPoint.transform;
                        GO.transform.position = WeaponAttachPoint.position;
                        GO.transform.rotation = WeaponAttachPoint.rotation;
                    }
                    else if (_weaponsInHand[0] != null){
                        //if we have a weapon already, put it in the secondary slot
                        _weaponsInHand.Add(GO.GetComponent<Weapon>());
                        _weaponsInHand[1].gameObject.SetActive(false);
                        GO.transform.parent = WeaponAttachPoint.transform;
                        GO.transform.position = WeaponAttachPoint.position;
                        GO.transform.rotation = WeaponAttachPoint.rotation;
                    }
                }
                if (GO.GetComponent<Gun>()){
                    ObjectPooler.ObjectPool.PoolItem(GO.GetComponent<Gun>().bullet);
                }
            }
            if (GO.GetComponent<Shield>()){
                
            }
        }

        /// <summary>
        /// swap a weapon on keypress
        /// </summary>
        private void WeaponSwap(){
            if (Input.GetKeyDown(KeyCode.Space) && _weaponsInHand.Count > 1){
                if (CurrentWeapon == _weaponsInHand[0]){
                    _weaponsInHand[0].gameObject.SetActive(false);
                    CurrentWeapon = _weaponsInHand[1];
                    _weaponsInHand[1].gameObject.SetActive(true);
                }
                else if (CurrentWeapon == _weaponsInHand[1]){
                    _weaponsInHand[1].gameObject.SetActive(false);
                    CurrentWeapon = _weaponsInHand[0];
                    _weaponsInHand[0].gameObject.SetActive(true);
                }
            }
        }
    }
}
