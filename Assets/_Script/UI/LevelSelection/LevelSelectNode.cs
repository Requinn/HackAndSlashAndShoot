using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using JLProject;
using System;

/// <summary>
/// Holds information about a node on the map
/// </summary>
public class LevelSelectNode : MonoBehaviour {
    [SerializeField]
    private string _LevelName = "LEVEL";
    [SerializeField]
    [TextArea]
    private string _LevelDescription = "Lorem Ipsum Dat.";
    [SerializeField]
    private int _sceneIndexToLoad;

    private Collider _interactionHitBox;
    //for sending data over on a hover
    public delegate void NodeSelectedEvent(LevelData data);
    public NodeSelectedEvent NodeSelected;
    //lock in that data
    public delegate void NodeLockInEvent(LevelData data);
    public NodeLockInEvent NodeLocked;
    public delegate void NodeDeselectedEvent();
    public NodeDeselectedEvent NodeLeft;

    private LevelData _data;
    private LevelData _emptyData = new LevelData();

    private void Start() {
        _interactionHitBox = GetComponent<Collider>();
        _interactionHitBox.enabled = false;
        _data = new LevelData(_LevelName, _LevelDescription, _sceneIndexToLoad);
    }

    public void SetHitBoxActive(bool enabled) {
        _interactionHitBox.enabled = enabled;
    }

    public LevelData GetData() {
        return _data;
    }

    private void OnMouseDown() {
        NodeLocked(GetData());
    }

    private void OnMouseEnter() {
        NodeSelected(GetData());
    }

    private void OnMouseExit() {
        NodeLeft();
    }
}

public struct LevelData {
    public string Name;
    public string Description;
    public int Scene;

    public LevelData(string name, string desc, int scene) {
        Name = name;
        Description = desc;
        Scene = scene;
    }
}
