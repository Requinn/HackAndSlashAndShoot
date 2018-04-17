using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

public class InvisibleWall : Toggleable {
    public override void Toggle(){
        throw new System.NotImplementedException();
    }

    public override void Open(){
        gameObject.SetActive(false);
    }

    public override void Close(){
        throw new System.NotImplementedException();
    }
}
