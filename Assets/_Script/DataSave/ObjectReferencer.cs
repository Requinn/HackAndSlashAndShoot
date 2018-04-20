using System;
using System.Collections;
using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;


/// <summary>
/// Holds a reference to objects assigned by an arbitrary ID.
/// </summary>
public class ObjectReferencer : MonoBehaviour {
    private static ObjectReferencer _instance = null;

    public static ObjectReferencer Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ObjectReferencer>();
                if (_instance == null) {
                    GameObject go = new GameObject(typeof(ObjectReferencer).ToString());
                    _instance = go.AddComponent<ObjectReferencer>();
                }
            }
            return _instance;
        }
    }

    void Awake() {
        if (Instance != this) {
            Destroy(this);
        }
        else {
            _instance = Instance;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Serializable]
    public struct ReferencedObj{
        public int ID;
        public GameObject obj;

        public ReferencedObj(int iD, GameObject go) {
            ID = iD;
            obj = go;
        }

    }

    public ReferencedObj[] ObjReferences;

    public GameObject FetchObjByID(int id){
        foreach (var o in ObjReferences){
            if (o.ID == id){
                return o.obj;
            }
        }
        return null;
    }

    public void AddObj(int ID, GameObject obj){
        ObjReferences.Add(new ReferencedObj(ID, obj));
    }
}
