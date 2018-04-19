using System.Collections;
using System.IO;
using System.Collections.Generic;
using DG.Tweening.Plugins;
using UnityEngine;

namespace JLProject {
    /// <summary>
    /// experimental method for getting the player to transition between scenes
    /// </summary>
    public class PlayerStats{
        //region defaults
        private const float DEFAULT_HEALTH = 200f;
        private const float DEFAULT_SHIELD = 80f;
        private const float DEFAULT_ARMOR = 0f;
        private const int DEFAULT_LEVEL = 0;

        //player vitals
        public float Health = DEFAULT_HEALTH;
        public float Shield = DEFAULT_SHIELD;
        public float Armor = DEFAULT_ARMOR;

        //player arms and armor
        public List<int> weapons = new List<int>();

        //misc. statistics
        public int lastLevel = DEFAULT_LEVEL;

        /// <summary>
        /// update all values of the save
        /// </summary>
        /// <param name="PC"></param>
        /// <param name="sceneNo"></param>
        public void UpdateStats(PlayerController PC, int sceneNo){
            Shield = PC.CurrentShield.CurrentHealth;
            Health = PC.CurrentHealth;
            Armor = PC.ArmorValue;
            lastLevel = sceneNo;
            foreach (var weap in PC.WeaponsInHand){
                weapons.Add(weap.ReferenceID);
            }
            DataService.Instance.WriteSaveData();
        }

        /// <summary>
        /// Writes the instance of this class as a JSON to filePath
        /// </summary>
        /// <param name="filePath"></param>
        public void WriteToFile(string filePath){
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// read a new playerstats obj from the data @ filepath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static PlayerStats ReadFromFile(string filePath){
            //if we don't have a save, return defaults
            if (!File.Exists(filePath)){
                return new PlayerStats();
            }
            else{
                //if it does, read the file to a string
                string contents = File.ReadAllText(filePath);

                //if it's empty, return a new save obj
                if (string.IsNullOrEmpty(contents)){
                    Debug.LogErrorFormat("File: '{0}' is empty.");
                    return new PlayerStats();
                }

                //otherwise, return the data we want
                return JsonUtility.FromJson<PlayerStats>(contents);
            }
        }
    }
}
