using MEC;
using UnityEngine;
using JLProject;
using Cinemachine;
using System;
using System.Collections.Generic;

public class LevelSelectMap : MonoBehaviour {
    [SerializeField]
    private LevelSelectNode[] _levelNodes;
    [SerializeField]
    private Toggleable _interactionkeyToggle;
    [SerializeField]
    private CinemachineVirtualCamera _mapCamera;
    [SerializeField]
    private LevelSelectUI _UIComponent;
    [SerializeField]
    private GameObject _healthUI;
	// Use this for initialization
	void Start () {
        _interactionkeyToggle.Open();
        _UIComponent.gameObject.SetActive(false);
        Timing.RunCoroutine(LateStart());
	}

    private IEnumerator<float> LateStart() {
        yield return Timing.WaitForOneFrame;
        foreach(var node in _levelNodes) {
            node.SetHitBoxActive(false);
            node.NodeSelected += UpdateUI;
            node.NodeLocked += LockUISelection;
            node.NodeLeft += ResetSelection;
        }
    }

    private void ResetSelection() {
        _UIComponent.ClearData();
    }

    private void LockUISelection(LevelData data) {
        _UIComponent.SetLockText(data);
    }

    ///<summary>
    ///update UI component with node data
    ///</summary>
    private void UpdateUI(LevelData data) {
        _UIComponent.SetText(data);
    }

    /// <summary>
    /// Open the map.
    /// </summary>
    private void OpenMap() {
        //FreezePlayer
        //change camera priority to map camera
        //enable colliders of all objects ont he map
        //enable level select ui
        GameController.Controller.PlayerReference.SetPlayerFrozen(true);
        _healthUI.SetActive(false);
        _interactionkeyToggle.Open();
        _mapCamera.Priority = 50;
        foreach(var node in _levelNodes) {
            node.SetHitBoxActive(true);
        }
        _UIComponent.gameObject.SetActive(true);
    }

    /// <summary>
    /// Called from a button in LevelSelectUI to close the map.
    /// </summary>
    public void CloseMap() {
        _UIComponent.HardClearData();
        _UIComponent.gameObject.SetActive(false);
        _interactionkeyToggle.Close();
        _healthUI.SetActive(true);
        foreach (var node in _levelNodes) {
            node.SetHitBoxActive(false);
        }
        _mapCamera.Priority = 0;
        GameController.Controller.PlayerReference.SetPlayerFrozen(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
            _interactionkeyToggle.Close();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E)) {
            OpenMap();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.Equals("Player")) {
            _interactionkeyToggle.Open();
        }
    }
}
