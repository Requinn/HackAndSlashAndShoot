using System.Collections;
using System.Collections.Generic;
using JLProject;
using MEC;
using UnityEngine;

public class BulletHell : MonoBehaviour{
    public int projectileCount = 12; //how many bullets do we shoot 
    public float projectileAngleRange = 120f; //how wide of an angle are they spread in
    public float fireRate = 1f; //how often
    public int fireCount = 1; //how many times
    public bool isFiring = false;
    public PlayerController targetPlayer;

    public GameObject projectile;
    private float angleStep;
    private float maxAngle;
    private float minAngle;
    private float curAngle;

    void Start(){
        ReCalculateAngle();
    }

    /// <summary>
    /// Used to update the attack properties from the boss's side
    /// </summary>
    /// <param name="pCount"></param>
    /// <param name="pAngle"></param>
    /// <param name="rof"></param>
    /// <param name="count"></param>
    public void UpdateProperties(int pCount, float pAngle, float rof, int count){
        projectileCount = pCount;
        projectileAngleRange = pAngle;
        fireRate = rof;
        fireCount = count;
        ReCalculateAngle();
    }

    /// <summary>
    /// re-acquire properties used in firing the attack
    /// </summary>
    void ReCalculateAngle(){
        angleStep = projectileAngleRange / projectileCount;
        maxAngle = projectileAngleRange / 2;
        minAngle = -maxAngle;
        curAngle = minAngle;
    }

    public void Fire(){
        Timing.RunCoroutine(FirePulse());
    }

    IEnumerator<float> FirePulse(){
        isFiring = true;
        for (int j = 0; j < fireCount; j++) {
            curAngle = minAngle;
            for (int i = 0; i <= projectileCount; i++) { //add +1 to make the pattern symmetrical??
                //Debug.Log(curAngle);
                GameObject go = Instantiate(projectile, transform.position,
                    Quaternion.Euler(0, transform.rotation.eulerAngles.y + curAngle, 0));
                go.GetComponent<Rigidbody>().velocity = go.transform.forward * 10f;
                Destroy(go, 5f);
                curAngle += angleStep;
            }
            yield return Timing.WaitForSeconds(fireRate);
        }
        isFiring = false;
        yield return 0f;
    }

	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.G)){
	        Fire();
	    }
        transform.LookAt(targetPlayer.transform);
	}
}
