using UnityEngine;

namespace JLProject {
    /// <summary>
    /// simepl switch to open and close a door
    /// TODO IMPROVE THE HECK OUT OF THIS???? (or keep it, it works) ((or maybe make it so all player attacks will do it, regardless of it being assigned or not))
    /// </summary>
    public class Switch : MonoBehaviour {
        public Unlockable object1;
        public Unlockable object2;
        public bool oneWay = false;
        public bool _on = false;
        
        public void Toggle(){
            if (!_on){
                object1.Open();
                if (object2){
                    object2.Close();
                }
                _on = true;
            }
            else if (_on && !oneWay){
                object1.Close();
                if (object2){
                    object2.Open();
                }
                _on = false;
            }
        }
        

    }
}