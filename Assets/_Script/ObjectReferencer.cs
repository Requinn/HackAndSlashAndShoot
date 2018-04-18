using System;
using System.Collections;
using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;

public class ObjectReferencer : MonoBehaviour{
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
