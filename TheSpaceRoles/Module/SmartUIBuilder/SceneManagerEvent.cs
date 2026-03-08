using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TSR.Module.SmartUIBuilder;

[MonoRegister.MonoRegister]
public class SceneManagerEvent : MonoBehaviour{
 
    public void Start () {
        SceneManager.sceneUnloaded += (UnityAction<Scene>)SceneUnloaded;
        
        return;

        void SceneUnloaded(Scene thisScene)
        {
            
        }
    }
    
}