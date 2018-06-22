using System.IO;
using UnityEngine;

namespace JLProject {
    /// <summary>
    /// Class made to handle all data IO for the game such as saving and loading.
    /// http://naplandgames.com/blog/2016/11/27/saving-data-in-unity-3d-serialization-for-beginners/
    /// </summary>
    public class DataService : MonoBehaviour{
        private static DataService _instance = null;

        public static DataService Instance{
            get{
                if (_instance == null){
                    _instance = FindObjectOfType<DataService>();
                    if (_instance == null){
                        GameObject go = new GameObject(typeof(DataService).ToString());
                        _instance = go.AddComponent<DataService>();
                    }
                }
                return _instance;
            }
        }

        //current playerstats
        public PlayerStats PlayerStats{ get; set; }

        //used to prevent reloading data everytime we load a new scene
        private bool isDataLoaded = false;

        public int curLoadedProfile{ get; set; }
        public const int MAX_SAVE_SLOTS = 1;

        void Awake(){
            if (Instance != this){
                Destroy(this);
            }
            else{
                _instance = Instance;
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// load save data based on profileNumber
        /// </summary>
        /// <param name="profileNumber"></param>
        public void LoadSaveData(int profileNumber = 0){
            if (isDataLoaded && profileNumber == curLoadedProfile){
                return;
            }
            //we didn't get a save to load
            if (profileNumber <= 0){
                //loop through all save slots to get the lowest slotted one
                for (int i = 1; i <= MAX_SAVE_SLOTS; i++){
                    if (File.Exists(GetSaveDataFilePath(i))){
                        PlayerStats = PlayerStats.ReadFromFile(GetSaveDataFilePath(i));
                        curLoadedProfile = i;
                        break;
                    }
                }
            }
            //we did get a save to load
            else{
                //our save exists, go get it
                if (File.Exists(GetSaveDataFilePath(profileNumber))){
                    PlayerStats = PlayerStats.ReadFromFile(GetSaveDataFilePath(profileNumber));
                }
                //our save doesn't exist, just make a new one
                else{
                    PlayerStats = new PlayerStats(); //I don't this actually does anything?????
                }
                curLoadedProfile = profileNumber;
            }
        }

        //our base filenames and directories
        private const string SAVE_DATA_FILENAME_BASE = "savedata";

        private const string SAVE_DATA_EXTENSION = ".txt";
        //done as a getter because our directory is nonconst to construct it
        private string SAVE_DATA_DIRECTORY{
            get{ return Application.dataPath + "\\saves\\"; }
        }

        /// <summary>
        /// return the full path and filename for the data
        /// ex 'C:\game\assets\saves\savedat1.txt'
        /// </summary>
        /// <param name="profileNumber"></param>
        /// <returns></returns>
        public string GetSaveDataFilePath(int profileNumber){
            if (profileNumber < 1){
                throw new System.ArgumentException("profileNumber < 1. Was: " + profileNumber);
            }

            if (!Directory.Exists(SAVE_DATA_DIRECTORY)){
                Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
            }

            return SAVE_DATA_DIRECTORY + SAVE_DATA_FILENAME_BASE + profileNumber + SAVE_DATA_EXTENSION;
        }

        /// <summary>
        /// write save data to a file
        /// </summary>
        public void WriteSaveData(){
            //if for some reason we didn't choose a profile number, check to see if we have any
            if (curLoadedProfile <= 0){
                for (int i = 1; i < MAX_SAVE_SLOTS; i++){
                    if (!File.Exists(GetSaveDataFilePath(i))){
                        curLoadedProfile = i;
                        break;
                    }
                }
            }

            if (curLoadedProfile <= 0){
                throw new System.Exception("Couldn't Save data.");
            }
            else{
                if (PlayerStats == null){
                    PlayerStats = new PlayerStats();
                }

                PlayerStats.WriteToFile(GetSaveDataFilePath(curLoadedProfile));
            }
        }
    }
}
