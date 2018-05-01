using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller for the player character
/// </summary>
namespace JLProject{
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

        private ParticleSystem _chargeParticle;
        void Awake(){
            floorMask = LayerMask.GetMask("Floor");
        }

        void Start(){
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

            CanMove = true;
            MovementSpeed = speed;
            cc = GetComponent<CharacterController>();
            _PAC = GetComponentInChildren<PlayerAnimationController>();
            _chargeParticle = GetComponentInChildren<ParticleSystem>();
            Faction = Damage.Faction.Player; 
        }

        // Update is called once per frame
        void Update(){
            //Debug.Log(MovementSpeed);
            if (!GameController.Controller.paused){
                if (_timeSinceAttack <= _attackDelay){
                    _timeSinceAttack += Time.deltaTime;
                }
                _MousePos = GetMousePosition();
                AngleUpdate(_MousePos);

                if (CanMove){
                    Movement();
                }
                MouseInput();
                WeaponSwap();
            }
        }

        /// <summary>
        /// gets mous input
        /// </summary>
        private void MouseInput(){
            //check for blocking
            if (Input.GetMouseButton(1) && CurrentShield != null){
                CancelCharge();
                if (!CurrentShield.broken){
                    if (!CurrentWeapon){
                        //we don't have a weapon, so block freely
                        CurrentShield.blocking = true;
                        AdjustSpeed(shieldedSpeed);
                    }
                    else{
                        if (CurrentWeapon._canBlock){
                            //we have a weapon, can't block if it can't attack, which is blocked by reloading or firing
                            CurrentShield.blocking = true;
                            AdjustSpeed(shieldedSpeed);
                        }
                    }
                }
            }
            //stop blocking
            else if (Input.GetMouseButtonUp(1) && CurrentShield != null){
                CancelCharge();
                ResetSpeed();
                CurrentShield.blocking = false;
            }
            //check for attacking
            if (Input.GetMouseButtonDown(0) && CurrentWeapon != null ) {
                if (CurrentShield != null && !CurrentShield.blocking){
                    if (CurrentWeapon._canAttack){
                        _PAC.GunShot();
                        CurrentWeapon.Fire();
                        /*if (CurrentWeapon.type == Weapon.Type.Melee || CurrentWeapon.GetComponent<BurstGun>()){   //if we're swinging a sword, stop our movement for delay seconds
                            _timeSinceAttack = 0.0f;
                            _attackDelay = CurrentWeapon.AttackDelay;
                        }
                    **/
                    }
                }
            }
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
        }

        ///cancels the current weapn charge
        private void CancelCharge(){
            _chargeTime = 0f;
            if (_chargeParticle.isPlaying) {
                _chargeParticle.Stop();
            }
        }
        /// <summary>
        /// character motor
        /// </summary>
        protected override void Movement(){
            _MovX = Input.GetAxisRaw("Horizontal");
            _MovZ = Input.GetAxisRaw("Vertical");
            _MoveDir = new Vector3(_MovX, 0, _MovZ).normalized;
            if(_PAC){
                _PAC.moveVector = _MoveDir;
            }
            _MoveDir *= MovementSpeed;
            cc.Move(_MoveDir * Time.deltaTime);
        }

        /// <summary>
        /// rotates the characters towards the mouse  
        /// </summary>
        private Vector3 GetMousePosition(){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //cast a ray to the mouse position in world
            Plane hPlane = new Plane(Vector3.up, transform.position);       //create a plane using the up vector and ourselves
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
                CancelCharge();
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
            }
        }
    }
}
