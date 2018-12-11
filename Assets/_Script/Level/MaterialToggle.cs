using UnityEngine.UI;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// Toggles the material and key indicator of an object when it can be interacted with.
    /// </summary>
    public class MaterialToggle : Toggleable{
        public Material[] toggledMaterials;
        [SerializeField]
        private GameObject _interactionKeyIndicator;
        private Renderer _rend;
        [SerializeField]
        private char _interactionKey = 'E';

        void Start(){
            _rend = GetComponent<Renderer>();
            if (_interactionKeyIndicator) {
                _interactionKeyIndicator.GetComponentInChildren<Text>().text = _interactionKey.ToString();
                _interactionKeyIndicator.SetActive(false);
            }
        }

        public override void Toggle(){
            if (Opened){
                Close();
            }
            else{
                Open();
            }
        }

        /// <summary>
        /// The switch is active, and ready to toggle.
        /// </summary>
        public override void Open(){
            _rend.material = toggledMaterials[0];
            if(_interactionKeyIndicator) _interactionKeyIndicator.SetActive(true);
            Opened = true;
        }

        /// <summary>
        /// The switch is not active, and not ready to be used.
        /// </summary>
        public override void Close(){
            _rend.material = toggledMaterials[1];
            if (_interactionKeyIndicator) _interactionKeyIndicator.SetActive(false);
            Opened = false;
        }
    }
}
