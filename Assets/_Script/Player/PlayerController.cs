﻿using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using JLProject.Weapons;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// A controller for the player character
    /// </summary>
    public class PlayerController : Entity{
        // Use this for initialization
        public bool useSave = true;
        public float speed, shieldedSpeed, chargingSpeed;
        private float _MovX, _MovZ;
        private CharacterController cc;
        private Vector3 _MoveDir = Vector3.zero; //not going anywhere
        private Vector3 _MousePos;
        private int floorMask;
        public float rotationSpeed = 360.0f;
        public Weapon CurrentWeapon;
        public Shield CurrentShield;
        public Transform WeaponAttachPoint;
        public Transform ShieldAttachPoint;

        public List<Weapon> WeaponsInHand = new List<Weapon>(2);
        private PlayerAnimationController _PAC;
        private float _timeSinceAttack = 0.0f;
        private float _attackDelay = 0.0f;
        public float _chargeTime = 0.0f;
        private float _height;
        public float FootRoot { get { return _height; } }
        private bool _isInputDisabled = false;

        private ParticleSystem _chargeParticle;

        public PickupObject PickupHandler;
        void Awake(){
            floorMask = LayerMask.GetMask("Floor");
            _PAC = GetComponentInChildren<PlayerAnimationController>();
        }

        void Start(){
            CanMove = true;
            MovementSpeed = speed;
            cc = GetComponent<CharacterController>();

            _height = transform.position.y - 0.95f;
            //_chargeParticle = GetComponentInChildren<ParticleSystem>();
            Faction = Damage.Faction.Player;
            if (useSave && DataService.Instance.curLoadedProfile == 1){
                CurrentHealth = DataService.Instance.PlayerStats.Health;
                if (UIhp){
                    UIhp.UpdateHealthBar(HealthPercent());
                }
                ArmorValue = DataService.Instance.PlayerStats.Armor;

                foreach (var wepID in DataService.Instance.PlayerStats.weapons){
                    Equip(Instantiate(ObjectReferencer.Instance.FetchObjByID(wepID)));
                }
            }
        }

        // Update is called once per frame
        void Update(){
            //Debug.Log(MovementSpeed);
            //as long as the game isn't pause, or we aren't frozen in place
            if (!GameController.Controller.paused){
                if (_timeSinceAttack <= _attackDelay){
                    _timeSinceAttack += Time.deltaTime;
                }
                //can't do anything since we are disabled so return
                if (_isInputDisabled) { return; }
                //get position of mouse to get our facing angle
                _MousePos = GetMousePosition();
                AngleUpdate(_MousePos);

                //check for movement
                Movement();

                //check to pick up an object
                if (PickupHandler.CanPickUp && Input.GetKeyDown(KeyCode.E)){
                    PickupHandler.PickUp();
                }

                //check if we are holding an object, any mouse input will throw it
                if (PickupHandler.currentObj){
                    if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)){
                        PickupHandler.Release();
                    }
                }
                else{
                    MouseInput();
                }

                WeaponSwap();
            }
        }

        /// <summary>
        /// gets mous input
        /// </summary>
        private void MouseInput(){
            ////check for blocking
            if (Input.GetMouseButton(1) && CurrentShield != null){
                CancelCharge();
                if (!CurrentShield.broken){
                    if (!CurrentWeapon){
                        //we don't have a weapon, so block freely
                        CurrentShield.blocking = true;
                        _PAC.PlayBlock();
                        AdjustSpeed(shieldedSpeed);
                    }
                    else{
                        if (CurrentWeapon._canBlock){
                            //we have a weapon, can't block if it can't attack, which is blocked by reloading or firing
                            CurrentShield.blocking = true;
                            _PAC.PlayBlock();
                            AdjustSpeed(shieldedSpeed);
                        }
                    }
                }
                else {
                    ResetSpeed();
                    CurrentShield.blocking = false;
                }
            }
            //stop blocking
            else if (Input.GetMouseButtonUp(1) && CurrentShield != null){
                CancelCharge();
                ResetSpeed();
                CurrentShield.blocking = false;
            }

            //TODO: Can this be better?
            //check for attacking
            if (CurrentWeapon != null){
                //single shot input
                if (!CurrentWeapon.isAutomatic){
                    if (CurrentShield != null && !CurrentShield.blocking) {
                        if (Input.GetMouseButtonDown(0) && CurrentWeapon._canAttack) {
                            if (CurrentWeapon is Gun) {
                                _PAC.PlayAttack((int)CurrentWeapon.Category);
                            }else {
                                _PAC.PlayMeleeAttack((int)CurrentWeapon.Category, CurrentWeapon.GetComponent<Melee>().CurrentCombo);
                            }
                            CurrentWeapon.Fire();
                            /**if (CurrentWeapon.type == Weapon.Type.Melee || CurrentWeapon.GetComponent<BurstGun>()){   //if we're swinging a sword, stop our movement for delay seconds
                                _timeSinceAttack = 0.0f;
                                _attackDelay = CurrentWeapon.AttackDelay;
                            }
                            **/
                        }
                    }
                }
                else if (CurrentWeapon.isAutomatic){
                    //automatic input
                    if (CurrentShield != null && !CurrentShield.blocking){
                        if (Input.GetMouseButton(0) && CurrentWeapon._canAttack){
                            _PAC.GunShot();
                            CurrentWeapon.Fire();
                        }
                        if (Input.GetMouseButtonUp(0)) {
                            CurrentWeapon.ResetWeapon();
                        }
                    }
                    //catchall for the only automatic weapon right now
                    if (Input.GetMouseButtonUp(0)) {
                        ResetSpeed();
                    }
                }
            }
            
            /**
            //handle Charge Attacks
            if (Input.GetMouseButton(0) && CurrentWeapon != null){
                if (_chargeTime < CurrentWeapon.ChargeTime){
                    _chargeTime += Time.deltaTime;
                }

                if (_chargeTime > 0.25f){
                    AdjustSpeed(chargingSpeed);
                }
                if (!_chargeParticle.isPlaying && Math.Abs(_chargeTime - CurrentWeapon.ChargeTime) < .01){
                    //var psMain = _chargeParticle.main;
                    //psMain.duration = CurrentWeapon.ChargeTime;
                    _chargeParticle.Play();
                }
            }
            //handle charge attack execution
            if (Input.GetMouseButtonUp(0) && CurrentWeapon != null){  
                if (_chargeTime >= CurrentWeapon.ChargeTime){
                    CurrentWeapon.ChargeAttack();
                }
                if (_chargeTime > 0.25f) {
                    ResetSpeed();
                }
                CancelCharge();
            }
            **/
            }

        ///cancels the current weapn charge
        private void CancelCharge(){
            _chargeTime = 0f;
        }

        /// <summary>
        /// Set whether the player is allowed to use inputs or not
        /// </summary>
        /// <param name="freeze"></param>
        public void SetPlayerFrozen(bool freeze = false) {
            _isInputDisabled = freeze;
            CanMove = !freeze;
            _PAC.moveVector = Vector3.zero;
        }

        /// <summary>
        /// character motor
        /// </summary>
        protected override void Movement(){
            //if we can move, apply movement calculated with look directions
            if (MovementSpeed > 0 && CanMove) {
                _MovX = Input.GetAxisRaw("Horizontal");
                _MovZ = Input.GetAxisRaw("Vertical");
                _MoveDir = new Vector3(_MovX, 0, _MovZ).normalized;
                if (_PAC) {
                    _PAC.moveVector = _MoveDir;
                }
                _MoveDir *= MovementSpeed;
                cc.Move(_MoveDir * Time.deltaTime);
            }
            //if we can't move, set motion vector to zero
            else if(!CanMove || MovementSpeed == 0) {
                if (_PAC) {
                    _PAC.moveVector = Vector3.zero;
                }
            }
        }

        /// <summary>
        /// rotates the characters towards the mouse  
        /// </summary>
        private Vector3 GetMousePosition(){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //cast a ray to the mouse position in world
            Plane hPlane = new Plane(Vector3.up, transform.position);       //create a plane using the up vector as its normal and our position
            float dist = 0.0f;
            if (hPlane.Raycast(ray, out dist)){                             //raycast onto our plane with our mouse input
                //OUT where our mouse sits on this plane and raycast the direction3 
                return ray.GetPoint(dist);
            }
            return Vector3.zero;
        }

        private Vector3 _pos, _dir;
        private Quaternion _lookrotation;
        /// <summary>
        /// updates our rotation
        /// </summary>
        /// <param name="tpos"></param>
        private void AngleUpdate(Vector3 tpos){
            _pos = transform.position;
            _dir = (tpos - _pos).normalized;
            _lookrotation = Quaternion.LookRotation(_dir);
            if (_PAC){
                _PAC.lookAngle = _lookrotation.eulerAngles.y;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookrotation, Time.deltaTime * rotationSpeed);
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
        /// DEBUG BUILDS ONLY
        /// </summary>
        protected override void HandleDeath() {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        public delegate void WeaponEquipEvent(Weapon primary, Weapon secondary);
        public event WeaponEquipEvent equipped;

        public void Equip(Weapon weapon, int slot) {
            Destroy(WeaponsInHand[slot]);
            WeaponsInHand[slot] = weapon;
            CurrentWeapon = weapon;
            CurrentWeapon.gameObject.SetActive(true);
            weapon.transform.parent = WeaponAttachPoint.transform;
            weapon.transform.position = WeaponAttachPoint.position;
            weapon.transform.rotation = WeaponAttachPoint.rotation;
        }
        /// <summary>
        /// Equips a GO to the attachpoint
        /// </summary>
        /// <param name="GO"></param>
        public void Equip(GameObject GO){
            if (GO.GetComponent<Weapon>()){
                //we have a weapon in both slots, replace the current by:
                //setting current weapon to the new instance, destroying the old one, then setting our current weapon to the one we just picked up
                if (WeaponsInHand.Count >= 2){
                    /**if (CurrentWeapon.type == Weapon.Type.Ranged){
                        ObjectPooler.ObjectPool.UnPoolItem(CurrentWeapon.GetComponent<Gun>().bullet);
                    }**/
                    WeaponsInHand[WeaponsInHand.IndexOf(CurrentWeapon)] = GO.GetComponent<Weapon>();
                    Destroy(CurrentWeapon.gameObject);
                    CurrentWeapon = GO.GetComponent<Weapon>();
                    CurrentWeapon.gameObject.SetActive(true);
                    GO.transform.parent = WeaponAttachPoint.transform;
                    GO.transform.position = WeaponAttachPoint.position;
                    GO.transform.rotation = WeaponAttachPoint.rotation;
                }
                else{
                    if (CurrentWeapon == null){
                        //if we don't have one
                        WeaponsInHand.Add(GO.GetComponent<Weapon>());
                        CurrentWeapon = WeaponsInHand[0];
                        CurrentWeapon.gameObject.SetActive(true);
                        GO.transform.parent = WeaponAttachPoint.transform;
                        GO.transform.position = WeaponAttachPoint.position;
                        GO.transform.rotation = WeaponAttachPoint.rotation;
                    }
                    else if (WeaponsInHand[0] != null){
                        //if we have a weapon already, put it in the secondary slot
                        WeaponsInHand.Add(GO.GetComponent<Weapon>());
                        WeaponsInHand[1].gameObject.SetActive(false);
                        GO.transform.parent = WeaponAttachPoint.transform;
                        GO.transform.position = WeaponAttachPoint.position;
                        GO.transform.rotation = WeaponAttachPoint.rotation;
                    }
                }
                if (equipped != null) equipped(WeaponsInHand[0], WeaponsInHand.Count < 2 ? null : WeaponsInHand[1]);
                //update the animator to use the correct layer
                if(_PAC) _PAC.SetActiveLayer((int)CurrentWeapon.Category);
            }
            if (GO.GetComponent<Shield>()){
                
            }
        }

        public delegate void WeaponSwapEvent();
        public event WeaponSwapEvent swapped;
        /// <summary>
        /// swap a weapon on keypress
        /// </summary>
        private void WeaponSwap(){
            if (Input.GetKeyDown(KeyCode.Space) && WeaponsInHand.Count > 1){
                //CancelCharge();
                CurrentWeapon.ResetWeapon();
                if (CurrentWeapon == WeaponsInHand[0]){
                    WeaponsInHand[0].gameObject.SetActive(false);
                    CurrentWeapon = WeaponsInHand[1];
                    WeaponsInHand[1].gameObject.SetActive(true);
                }
                else if (CurrentWeapon == WeaponsInHand[1]){
                    WeaponsInHand[1].gameObject.SetActive(false);
                    CurrentWeapon = WeaponsInHand[0];
                    WeaponsInHand[0].gameObject.SetActive(true);
                }
                if (swapped != null) swapped();
                //update animator to use correct layer
                _PAC.SetActiveLayer((int)CurrentWeapon.Category);
                _PAC.PlayWeaponSwap((int)CurrentWeapon.Category);
            }
        }
    }
}
