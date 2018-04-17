using UnityEngine;

namespace JLProject{
    /// <summary>
    /// simple trigger to start up a level objective
    /// </summary>
    public class SimpleTrigger:MonoBehaviour{
        public LevelObjective objective;

        void OnTriggerEnter(Collider c){
            if (c.gameObject.tag == "Player"){
                //GameController.Controller.dialogManager.WriteText("It was a trap!",0 , 0.01f, 3f);
                objective.Initiate();
            }
        }
    }
}