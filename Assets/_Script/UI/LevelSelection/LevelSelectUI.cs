using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JLProject;
using UnityEngine.UI;

/// <summary>
/// Class to handle the UI element of level selection
/// </summary>
public class LevelSelectUI : MonoBehaviour {
    [SerializeField]
    private Text _levelDescriptionText;
    [SerializeField]
    private Text _levelName;
    [SerializeField]
    private Button _loadLevelButton;

    private string _lockedDesc, _lockedName;
    private int _lockedLevelIndex;

    /// <summary>
    /// Clear out the text stored in the fields.
    /// </summary>
    public void HardClearData() {
        _lockedDesc = "";
        _lockedName = "";
        _lockedLevelIndex = 0;
        _levelDescriptionText.text = "";
        _levelName.text = "";
        _loadLevelButton.gameObject.SetActive(false);
    }

    public void ClearData() {
        _levelDescriptionText.text = _lockedDesc;
        _levelName.text = _lockedName;
        //if we have data locked in, return the button
        if(_lockedLevelIndex != 0) {
            _loadLevelButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// set the UI text from the node data temporarily
    /// </summary>
    /// <param name="data"></param>
    public void SetText(LevelData data) {
        _levelDescriptionText.text = data.Description;
        _levelName.text = data.Name;
        if (_lockedLevelIndex != data.Scene) {
            _loadLevelButton.gameObject.SetActive(false);
        }
    }
    
    public void SetLockText(LevelData data) {
        _lockedDesc = data.Description;
        _lockedName = data.Name;
        _lockedLevelIndex = data.Scene;
        _loadLevelButton.gameObject.SetActive(true);
    }

    public void LoadLevel() {
        if (_lockedLevelIndex != 0) {
            SceneLoader.Instance.LoadLevel(_lockedLevelIndex);
        }
    }

}
