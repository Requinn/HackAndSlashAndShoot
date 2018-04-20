using HutongGames.PlayMaker.Actions;
using UnityEngine;
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
        private PauseMenu _pauseMenu;

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
            //world triggers
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
                Cursor.lockState = CursorLockMode.None;
                paused = true;
                pauseUI.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else{
                Cursor.lockState = CursorLockMode.Confined;
                paused = false;
                pauseUI.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
}