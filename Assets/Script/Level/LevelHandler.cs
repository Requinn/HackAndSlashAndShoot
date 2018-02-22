using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

/// <summary>
/// DONT USE THIS FOR ANYTHING!!!!!!!!!!!!
/// </summary>
public class LevelHandler : MonoBehaviour{
    public GameObject LevelExit;
    public List<LevelObjective> Objectives = new List<LevelObjective>();
    public List<bool> _objectiveCompletion = new List<bool>(3);

    /// <summary>
    /// subscribe to completion events
    /// and set the list of completed objects to false
    /// </summary>
    void Start(){
        LevelExit.SetActive(false);
        for (int i = 0; i > Objectives.Count; i++){
            //Objectives[i].OnCompleteObjective += MarkComplete;
            _objectiveCompletion.Add(false);
        }
    }

    /// <summary>
    /// mark objective #index as completed
    /// </summary>
    /// <param name="order"></param>
    private void MarkComplete(int order){
        _objectiveCompletion[order] = true;
        CheckCompletion();
    }

    /// <summary>
    /// check if we did everything
    /// </summary>
    private void CheckCompletion(){
        foreach (var completed in _objectiveCompletion){
            if (!completed) {
                return;
            }
        }
        EnableExit();
    }

    /// <summary>
    /// turn the exit on
    /// </summary>
    private void EnableExit(){
        LevelExit.SetActive(true);
    }

}
