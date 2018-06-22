using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace JLProject{
    /// <summary>
    /// Handles the mouse reticle/cursor
    /// </summary>
    public class MouseReticle : MonoBehaviour{
        public Texture2D reticleImage = null;

        // Use this for initialization
        void Start(){
            Cursor.SetCursor(reticleImage, new Vector2(reticleImage.width/2f, reticleImage.height/2f), CursorMode.Auto);
        }

        /// <summary>
        /// set cursor the default pointer
        /// </summary>
        public void SetCursorDefault(){
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        /// <summary>
        /// set cursor to a custom one, or the default assigned when not supplied
        /// </summary>
        /// <param name="img"></param>
        public void SetCursorCustom(Texture2D img = null){
            if (img == null){
                Cursor.SetCursor(reticleImage, new Vector2(reticleImage.width / 2f, reticleImage.height / 2f),
                    CursorMode.Auto);
            }
            else{
                Cursor.SetCursor(img, new Vector2(img.width / 2f, img.height / 2f), CursorMode.Auto);
            }
        }
    }
}
