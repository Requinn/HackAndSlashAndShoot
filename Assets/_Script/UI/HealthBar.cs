using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour{
    public Image healthBar;
    public Image lerpedHealthBar; //this healthbar is going to be used to show the effect when taking damage
    public float lerpSpeed = 10f; //how fast do we lerp
    public float lerpDelay = .35f; //How soon after taking damage do we lerp
    private bool _canLerp = false;
    private CoroutineHandle delayHandle;
    private float TargetValue {
        get { return healthBar.fillAmount; }
    }
    private float _originalHealth;

    public void UpdateHealthBar(float newHp){
        //Scales the foreground of the healthbar to the value of hp clamped between 0 and 1
        /*healthBar.transform.localScale = new Vector3(Mathf.Clamp(hp, 0f, 1f), healthBar.transform.localScale.y,
            healthBar.transform.localScale.z);*/
        healthBar.fillAmount = newHp;
        //if we are alive, do the standard delay then lerp, only resetting when we get hit again
        if (newHp > 0) {
            _canLerp = false;
            Timing.KillCoroutines(delayHandle);
            delayHandle = Timing.RunCoroutine(SetLerp());
        }
        //if we die, ignore everything and fall to 0 quickly
        else {
            _canLerp = true;
            lerpSpeed *= 1.35f;
        }
        
    }

    /// <summary>
    /// Lerp from original health value to new value
    /// </summary>
    public void Update() {
        if (lerpedHealthBar && _canLerp) {
            lerpedHealthBar.fillAmount = Mathf.Lerp(lerpedHealthBar.fillAmount, TargetValue, Time.deltaTime * lerpSpeed);
            if(Mathf.Abs(lerpedHealthBar.fillAmount - TargetValue) <= 0.001f) {
                _canLerp = false;
            }
        }
    }

    IEnumerator<float> SetLerp() {
        yield return Timing.WaitForSeconds(lerpDelay);
        _canLerp = true;
        yield return 0f;
    }
}
