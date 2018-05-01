using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public class MaterialToggle : Toggleable{
        public Material[] toggledMaterials;
        private Renderer _rend;

        void Start(){
            _rend = GetComponent<Renderer>();
        }

        public override void Toggle(){
            if (Opened){
                Close();
            }
            else{
                Open();
            }
        }

        public override void Open(){
            _rend.material = toggledMaterials[0];
            Opened = true;
        }

        public override void Close(){
            _rend.material = toggledMaterials[1];
            Opened = false;
        }
    }
}
