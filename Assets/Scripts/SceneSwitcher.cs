using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SceneSwitcher : MonoBehaviour
{
    public PostProcessVolume menuPostProcess;
    public PostProcessVolume gamePostProcess;

    void Start()
    {
        menuPostProcess.enabled = true;
        gamePostProcess.enabled = false;
    }

    public void SwitchToGameScene()
    {
        menuPostProcess.enabled = false;
        gamePostProcess.enabled = true;
    }
}
