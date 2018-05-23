using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using JLProject;
using MEC;
using UnityEngine;

public class FadingBlockade : Switch{
    [SerializeField] private bool _primer;
    [SerializeField] private float _fadeTimer = 1.0f;

    private bool _canFade, _struck;
    private Renderer _renderer;
    private Color _col;
    private Material _mat;

    void Start(){
        _renderer = GetComponent<Renderer>();
        _mat = _renderer.material;
        _col = _mat.color;
    }

    public override void Toggle(){
        Debug.Log("AAA");
        if (_primer){
            StartChain();
        }
    }

    void Update(){
        if (_canFade) {
            ///_mat.color = new Color(_col.r, _col.g, _col.b, Mathf.Clamp(_mat.color.a - (_fadeTimer * Time.deltaTime) , 0.15f, 1f));
        }
    }

    private void StartChain(){
        ContinueChain();
    }

    private void DisableSelf(){
        gameObject.SetActive(false);
    }

    //ray cast in all directions
    public void ContinueChain(){
        Debug.Log(gameObject.name);
        //start fade
        _canFade = true;
        Timing.RunCoroutine(DelayedSendcast());
    }

    IEnumerator<float> DelayedSendcast(){
        RaycastHit hit;
        yield return Timing.WaitForSeconds(_fadeTimer);

        //shoot out a raycast forward
        if (Physics.Raycast(new Ray(transform.position, Vector3.forward), out hit, 1.5f)) {
            if (hit.transform != null) {
                hit.transform.gameObject.SendMessage("ContinueChain", SendMessageOptions.DontRequireReceiver);
            }
        }

        //back
        if (Physics.Raycast(new Ray(transform.position, Vector3.back), out hit, 1.5f)) {
            if (hit.transform != null) {
                hit.transform.gameObject.SendMessage("ContinueChain", SendMessageOptions.DontRequireReceiver);
            }
        }

        //right
        if (Physics.Raycast(new Ray(transform.position, Vector3.right), out hit, 1.5f)) {
            if (hit.transform != null) {
                hit.transform.gameObject.SendMessage("ContinueChain", SendMessageOptions.DontRequireReceiver);
            }
        }

        //left
        if (Physics.Raycast(new Ray(transform.position, Vector3.left), out hit, 1.5f)) {
            if (hit.transform != null) {
                hit.transform.gameObject.SendMessage("ContinueChain", SendMessageOptions.DontRequireReceiver);
            }
        }
        DisableSelf();
        yield return 0f;
    }
}
