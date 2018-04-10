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
        [SerializeField]
        public enum ObjectiveType{
            Kill,
            Reach,
            Trigger

        }

        public Unlockable ObjectToUnlock;
        public Unlockable ObjectToLock;

        protected Action onCompleteObjective = delegate { };
        public Action OnCompleteObjective {
            get { return onCompleteObjective; }
            set { onCompleteObjective = value; }
        }
        public abstract void Initiate();
    }
}
