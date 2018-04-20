using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace JLProject{
    public class MainMenuSaveSelect : MonoBehaviour{
        public GameObject ContinueButton;
        public GameObject WarningText;
        private bool _hasSave = false; 
        /// <summary>
        /// check for a save file
        /// </summary>
        void Awake(){
            _hasSave = File.Exists(DataService.Instance.GetSaveDataFilePath(1));
        }

        // Use this for initialization
        void OnEnable(){
            SetButtonLabels();
        }

        /// <summary>
        /// enable the continue button if we have a file
        /// </summary>
        private void SetButtonLabels(){
            if (_hasSave){
                ContinueButton.SetActive(true);
            }
            else{
                ContinueButton.SetActive(false);
            }
        }

        /// <summary>
        /// start a new game, warn the player if we have found a save
        /// </summary>
        public void LoadNew() {
            if (_hasSave){
                ShowWarning();
            }
            else{
                SceneLoader.Instance.LoadLevel(1);
            }
        }

        /// <summary>
        /// delete that and continue
        /// </summary>
        public void DeleteSaveAndContinue(){
            HideWarning();
            File.Delete(DataService.Instance.GetSaveDataFilePath(1));
            SceneLoader.Instance.LoadLevel(0);
        }

        private void ShowWarning(){
            WarningText.SetActive(true);
        }

        public void HideWarning(){
            WarningText.SetActive(false);
        }

        public void LoadContinue() {
            SceneLoader.Instance.LoadLevel(DataService.Instance.PlayerStats.lastLevel);
        }

    }
}
