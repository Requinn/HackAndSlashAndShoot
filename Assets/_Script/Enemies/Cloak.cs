using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class Cloak : MonoBehaviour{
    public float duration = 5f;
    public bool isCloaked = false;
    public SkinnedMeshRenderer rend;
    public Material[] materials;
    public GameObject[] objectsToHide; //array of misc stuff to hide, like nameplates etc

    private CoroutineHandle _cloakHandle;

    /// <summary>
    /// Start hiding
    /// </summary>
    public void StartCloak(){
        isCloaked = true;
        _cloakHandle = Timing.RunCoroutine(InitCloak());
    }

    /// <summary>
    /// Hide all relevant objects with the mesh itself for duration seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> InitCloak(){
        rend.material = materials[1];
        foreach (var obj in objectsToHide) {
            Renderer r = obj.GetComponent<Renderer>();
            Canvas c = obj.GetComponent<Canvas>();
            if (r) {
                r.enabled = false;
            }
            else if (c){
                c.enabled = false;
            }
            else {
                obj.SetActive(false);
            }
        }
        yield return Timing.WaitForSeconds(duration);
        EndCloak();
    }

    /// <summary>
    /// reveal everything
    /// </summary>
    public void EndCloak(){
        isCloaked = false;
        Timing.KillCoroutines(_cloakHandle);
        foreach (var obj in objectsToHide) {
            if (!this) {
                return;
            }
            Renderer r = obj.GetComponent<Renderer>();
            Canvas c = obj.GetComponent<Canvas>();
            if (r) {
                r.enabled = true;
            }
            if (c){
                c.enabled = true;
            }
            else {
                obj.SetActive(true);
            }
        }
        rend.material = materials[0];
    }
}
