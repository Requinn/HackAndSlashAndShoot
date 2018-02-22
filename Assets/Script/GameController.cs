using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Controller != null){
            GameObject.Destroy(Controller);
        }
        else{
            Controller = this;
        }

        DontDestroyOnLoad(this); //this instance will persist through scenes

        Cursor.lockState = CursorLockMode.Confined;
        //pause menu stuff
        _pauseMenu = pauseUI.GetComponent<PauseMenu>();
        _pauseMenu.Restart += RestartLevel;
        _pauseMenu.LoadNext += LoadNextLevel;

        //world triggers
    }

	// Update is called once per frame
	void Update (){
	    Pause();
	}

    /// <summary>
    /// restart the level
    /// </summary>
    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    /// <summary>
    /// go to the next level
    /// </summary>
    public void LoadNextLevel(){
        //just call restart level for now
        RestartLevel();
    }

    //Move this to the pause handler
    /// <summary>
    /// pause the game
    /// </summary>
    private void Pause(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!paused) {
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
