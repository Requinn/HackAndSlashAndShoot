using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public abstract class Toggleable : MonoBehaviour{
        public bool Locked = false;
        public bool Opened = false;

        public virtual void Toggle() {
            if (Opened) {
                Close();
            }
            else if (!Opened) {
                Open();
            }
        }
        public abstract void Open();
        public abstract void Close();

    }
}

