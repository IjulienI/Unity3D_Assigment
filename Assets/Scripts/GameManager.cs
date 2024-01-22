using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool maskOn;
    public bool detectable;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (maskOn)
        {
            detectable = true;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), Mathf.RoundToInt(1/Time.deltaTime).ToString());
    }
}
