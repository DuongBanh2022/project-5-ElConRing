using UnityEngine;
public class QuitGame : MonoBehaviour
{
    public bool stopPlayModeInEditor = false;
    public void QuitToDesktop()
    {
        Debug.Log("Quit requested");
        Application.Quit();
#if UNITY_EDITOR
        if (stopPlayModeInEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
    }
}