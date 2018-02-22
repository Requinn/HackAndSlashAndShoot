using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public abstract class Unlockable : MonoBehaviour{
        private bool locked = false;
        public bool Locked{
            get{ return locked; }
            set{ locked = value; }
        }
        public abstract void Open();
        public abstract void Close();

    }
}

