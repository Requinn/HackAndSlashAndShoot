using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// handles objects that are breakable
    /// </summary>
    public abstract class BreakableObject : MonoBehaviour{
        [SerializeField] private int _hitsToBreak = 1;

        public virtual void Hit(){
            _hitsToBreak--;
            if (_hitsToBreak == 0){
                Break();
            }
            Debug.Log(_hitsToBreak);
        }

        public virtual void Break(){
            gameObject.SetActive(false);
        }

    }
}
