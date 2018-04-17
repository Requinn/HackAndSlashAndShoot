using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

public class InteractObjective : MonoBehaviour{
    public List<Interactable> Interactables;
    public Toggleable objToOpen;
    public DialogueTrigger respondingNPC;
    private int _minimumCount, _currentCount;

    void Start(){
        foreach (var interacts in Interactables){
            interacts.Interracted += AddCount;
        }
        _minimumCount = Interactables.Count;
    }

    void AddCount(){
        _currentCount++;
        CheckCount();
    }

    void CheckCount(){
        if (_currentCount >= _minimumCount){
            objToOpen.Open();
            if(respondingNPC) respondingNPC.ChangeResponse();
        }
    }

}
