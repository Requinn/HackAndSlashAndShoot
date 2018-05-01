using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public abstract class Toggleable : MonoBehaviour{
        private bool locked = false;
        public bool Opened = false;
        public bool Locked{
            get{ return locked; }
            set{ locked = value; }
        }

        public abstract void Toggle();
        public abstract void Open();
        public abstract void Close();

    }
}

