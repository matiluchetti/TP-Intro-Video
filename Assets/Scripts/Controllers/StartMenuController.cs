using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour {
    public void onExitClick() {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
