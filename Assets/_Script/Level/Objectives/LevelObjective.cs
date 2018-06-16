using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

/// <summary>
/// base class for level objectives
/// </summary>
namespace JLProject{
    public abstract class LevelObjective : MonoBehaviour{
        public bool oneWay = false;
        protected bool isObjectiveComplete;
        public bool isCompleted {
            get { return isObjectiveComplete; }
        }

        [SerializeField]
        public enum ObjectiveType{
            Kill,
            Reach,
            Trigger
        }

        public Toggleable ObjectToUnlock; //what is unlocked at the end of the objective
        public Toggleable ObjectToLock; //what is locked at the beginning of the objective

        protected Action onCompleteObjective = delegate { };
        public Action OnCompleteObjective {
            get { return onCompleteObjective; }
            set { onCompleteObjective = value; }
        }

        /// <summary>
        /// start the objective
        /// </summary>
        public virtual void Initiate(){
            if (ObjectToLock) {
                ObjectToLock.Locked = true;
                ObjectToLock.Close();
            }
        }

        /// <summary>
        /// end the objective and open the doors
        /// </summary>
        protected virtual void OpenDoors(){
            if (ObjectToUnlock) {
                ObjectToUnlock.Locked = false;
                ObjectToUnlock.Open();
            }
            if (!oneWay && ObjectToLock) {
                ObjectToLock.Locked = false;
                ObjectToLock.Open();
            }
        }
    }
}
