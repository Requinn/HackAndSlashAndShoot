﻿using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JLProject{
    public class SceneLoader : MonoBehaviour{
        public static SceneLoader Instance;
        public event Action<float> OnStartLoad = delegate(float duration) { };
        public event Action<float> OnEndLoad = delegate (float duration) { };

        public float minimumWaitTime = 1f;
        public float postReadyDelay = 1f;
        private bool loadScene = false;

        private void Awake(){
            Instance = this;
        }

        /// <summary>
        /// autosave and go to the next level
        /// </summary>
        public void LoadNextLevel(){
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            DataService.Instance.PlayerStats.UpdateStats(FindObjectOfType<PlayerController>(), nextScene);
            LoadLevel(nextScene);
        }

        /// <summary>
        /// go to the specified level
        /// </summary>
        /// <param name="sceneIndex"></param>
        public void LoadLevel(int sceneIndex){
            if (!loadScene){
                loadScene = true;
                OnStartLoad(minimumWaitTime); //fire off an event for a loading screen or something
                Timing.RunCoroutine(CoLoadScene(sceneIndex));

            }
        }

        private IEnumerator<float> CoLoadScene(int index){
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index); //load scene async
            asyncLoad.allowSceneActivation = false;

            yield return Timing.WaitForSeconds(minimumWaitTime);

            while(asyncLoad.progress < 0.89f){
                Debug.Log(asyncLoad.progress);
                yield return 0f; //spinlock my dudes
            }

            OnEndLoad(postReadyDelay);
            yield return Timing.WaitForSeconds(postReadyDelay);
            asyncLoad.allowSceneActivation = true;
        }
    }
}