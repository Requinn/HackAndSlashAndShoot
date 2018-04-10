using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLProject;

public class PauseMenu : MonoBehaviour{
    public delegate void PauseEvent();
    public event PauseEvent Restart, LoadNext, LoadMain, ResumeGame;

    public void Start(){
        gameObject.SetActive(false);
    }

    public void RestartLevel(){
        Restart();
    }

    public void LoadNextLevel(){
        LoadNext();
    }

    public void MainMenu(){
        LoadMain();
    }

    public void Resume(){
        ResumeGame();
    }
}
