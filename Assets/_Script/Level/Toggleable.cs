using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public abstract class Toggleable : MonoBehaviour{
        public bool Locked = false;
        public bool Opened = false;

        public abstract void Toggle();
        public abstract void Open();
        public abstract void Close();

    }
}

