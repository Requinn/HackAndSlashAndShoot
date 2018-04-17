using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace JLProject{
    public class DialogUI : MonoBehaviour{
        [SerializeField]
        private GameObject[] _textArea;

        private bool _isTyping = false;
        private int _currentLetter;
        private CoroutineHandle typingHandle;
        /// <summary>
        /// sets the text of _textArea[textBox] to text with typeSpeed, lasting for textDuration after finished typing
        /// TODO: I don't know if this actually works?
        /// TODO: Look into accessing this from a global area or someshit
        /// </summary>
        /// <param name="text"></param>
        /// <param name="typeSpeed"></param>
        /// <param name="textDuration"></param>
        public void WriteText(string text, int textBox, float typeSpeed, float textDuration){
            Timing.KillCoroutines(typingHandle);
            _currentLetter = 0;
            _textArea[textBox].SetActive(true);
            typingHandle = Timing.RunCoroutine(StartTyping(text, textBox, typeSpeed, textDuration));
        }

        private IEnumerator<float> StartTyping(string text, int t, float typeSpeed, float textDuration) {
            _isTyping = true;
            Text writeBox = _textArea[t].GetComponentInChildren<Text>();
            writeBox.text = "";
            while (_isTyping && (_currentLetter < text.Length - 1)){
                writeBox.text += text[_currentLetter];
                _currentLetter++;
                yield return Timing.WaitForSeconds(typeSpeed);
            }
            writeBox.text = text;   
            yield return Timing.WaitForSeconds(textDuration);
            writeBox.text = "";
            _textArea[t].SetActive(false);
            _isTyping = false;
        }

        // Update is called once per frame
        void Update(){

        }
    }
}
