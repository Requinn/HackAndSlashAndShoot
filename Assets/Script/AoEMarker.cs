using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using UnityEngine;

public class AoEMarker : MonoBehaviour{
    public GameObject Marker, Max;
    public Shape shape;
    public enum Shape{
        Circle,
        Rectangle
    }

    public void StartCast(float castTime, float AoESize){
        Max.SetActive(true);
        Marker.SetActive(true);
        Timing.RunCoroutine(DelayedDisable(castTime));
        Max.transform.localScale = new Vector3(1, 0.01f, 0);
        Marker.transform.localScale = new Vector3(1,0.01f,0);
        if (shape == Shape.Circle){
            if (Max){
                Max.transform.DOScale(new Vector3(AoESize * 2, 0.01f, AoESize * 2), 0.1f);
            }
            if (Marker){
                Marker.transform.DOScale(new Vector3(AoESize * 2, 0.01f, AoESize * 2), castTime);
            }
        }
        if (shape == Shape.Rectangle){
            if (Max) {
                Max.transform.DOScaleZ(AoESize, 0.1f);
            }
            if (Marker) {
                Marker.transform.DOScaleZ(AoESize, castTime);
            }
        }
    }

    private IEnumerator<float> DelayedDisable(float castTime){
        yield return Timing.WaitForSeconds(castTime);
        if (this){
            Max.SetActive(false);
            Marker.SetActive(false);
        }
    }
}
