using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    /// <summary>
    /// handles objects that should respond to camera obstruction by fading
    /// </summary>
    public class ObjectCameraFader : MonoBehaviour{
        [SerializeField] private float FadeRate = 2.5f;
        private bool _canFade = false;
        private Renderer _renderer;
        private Color _col;
        private Material _mat;

        // Use this for initialization
        void Start(){
            _renderer = GetComponent<Renderer>();
            _mat = _renderer.material;
            _col = _mat.color;
        }

        // Update is called once per frame
        void Update(){
            if (_canFade){
                _mat.color = new Color(_col.r, _col.g, _col.b, Mathf.Clamp(_mat.color.a - (FadeRate * Time.deltaTime), 0.1f, 1f));
            }
            else
            {
                _mat.color = new Color(_col.r, _col.g, _col.b, Mathf.Clamp(_mat.color.a + (FadeRate * Time.deltaTime), 0.1f, 1f));
            }
            _canFade = false;
        }

        public void EnableFade(){
            _canFade = true;
        }
    }
}