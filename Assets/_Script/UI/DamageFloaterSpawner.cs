using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// instantiates a floating text number related to the amount of damage taken or healed
/// </summary>
public class DamageFloaterSpawner : MonoBehaviour{
    public float displayDamage = 0.0f;
    
    public float alphaValue = 1.0f;
    public Color textColor = Color.grey; //default case
    public Transform UIobj;
    private Text _text;

    private Color _damagedColor = Color.red;  //health damage
    private Color _shieldedColor = Color.cyan; //shield damahe
    private Color _immuneColor = Color.grey; //no damage
    private Color _healedColor = Color.green;  // heal color

    void Awake(){
        _text = UIobj.GetComponent<Text>();
    }

    /// <summary>
    /// spawn a floating text object that will display damage, healing, or immunity
    /// </summary>
    /// <param name="damageIn"></param>
    public void SpawnDamageText(float damageIn){
        if (damageIn != 0){
            _text.color = _damagedColor;
            _text.text = "-" + Mathf.Floor(damageIn);
        }
        else{
            _text.color = _immuneColor;
            _text.text = "Immune!";
        }
        Instantiate(UIobj, transform.position, transform.rotation, transform);
    }

    public void SpawnHealText(float healIn){
        _text.color = _healedColor;
        _text.text = "+" + Mathf.Floor(healIn);
        Instantiate(UIobj, transform.position, transform.rotation, transform);
    }

    public void SpawnShieldText(float shieldIn){
        _text.color = _shieldedColor;
        _text.text = "-" + Mathf.Floor(shieldIn);
        Instantiate(UIobj, transform.position, transform.rotation, transform);
    }
}
