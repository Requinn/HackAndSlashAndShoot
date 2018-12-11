using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLProject{
    public class DialogueTrigger : MonoBehaviour{
        private string text = "";
        [TextArea]
        public string initText = "You shouldn't be seeing this text.";
        [TextArea]
        public string responseText = "You're good to go";
        public float typeSpeed = 0.01f;
        public int dialogBoxID = 0;
        public float postTypeDuration = 3f;
        public DialogUI dialogManager;
        void Start(){
            text = initText;
        }

        public void ChangeResponse(){
            text = responseText;
        }

        void OnTriggerEnter(Collider c) {
            if (c.gameObject.tag == "Player" && text != string.Empty){
                dialogManager.WriteText(text , dialogBoxID , typeSpeed, postTypeDuration);
            }
        }
    }
}
