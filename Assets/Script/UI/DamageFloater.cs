using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// a short lived object for damage text floaters
/// </summary>
public class DamageFloater : MonoBehaviour{
    public float alpha;
    public float duration = 0.5f; // how long text stays
    public float scrollSpeed = 0.05f; // speed text moves

    void Awake (){
	    alpha = GetComponent<Text>().material.color.a;
        DestroyObject(gameObject, duration);
	}
	
	// Update is called once per frame
	void Update () {
	    transform.position += scrollSpeed * Vector3.up;
       
	}
}
