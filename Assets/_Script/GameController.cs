using System;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace JLProject{
    /// <summary>
    /// A singleton to control the game
    /// </summary>
    public class GameController : MonoBehaviour{
        public static GameController Controller;
        public GameObject soundManager;
        public bool paused = false;
        public GameObject pauseUI;
        public MouseReticle reticle;
        private PauseMenu _pauseMenu;
        public PlayerController PlayerReference;

        void Awake(){
            if (Controller != null && Controller != this){
                Destroy(Controller);
            }
            else{
                Controller = this;
                DontDestroyOnLoad(this); //this instance will persist through scenes
            }
            DataService.Instance.LoadSaveData(1);

            Cursor.lockState = CursorLockMode.Confined;
            //pause menu stuff
            _pauseMenu = pauseUI.GetComponent<PauseMenu>();
            _pauseMenu.Restart += RestartLevel;
            _pauseMenu.LoadMain += LoadMain; 
            _pauseMenu.ResumeGame += TogglePause;
            PlayerReference = FindObjectOfType<PlayerController>();
            SceneManager.activeSceneChanged += UpdatePlayerReference;

            //world triggers
        }

        private void UpdatePlayerReference(Scene arg0, Scene arg1) {
            PlayerReference = FindObjectOfType<PlayerController>();
        }

        // Update is called once per frame
        void Update(){
            Pause();
        }

        public void LoadMain(){
            SceneManager.LoadScene(0);
            paused = false;
            pauseUI.SetActive(false);
            Destroy(this);
            Time.timeScale = 1.0f;
        }

        /// <summary>
        /// restart the level
        /// </summary>
        public void RestartLevel(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerReference = FindObjectOfType<PlayerController>();
            Cursor.lockState = CursorLockMode.Confined;
            paused = false;
            pauseUI.SetActive(false);
            Time.timeScale = 1.0f;
        }

        //Move this to the pause handler
        /// <summary>
        /// pause the game
        /// </summary>
        private void Pause(){
            if (Input.GetKeyDown(KeyCode.Escape)){
                TogglePause();
            }
        }

        private void TogglePause(){
            if (!paused){
                reticle.SetCursorDefault();
                Cursor.lockState = CursorLockMode.None;
                paused = true;
                pauseUI.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else{
                reticle.SetCursorCustom();
                Cursor.lockState = CursorLockMode.Confined;
                paused = false;
                pauseUI.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
}