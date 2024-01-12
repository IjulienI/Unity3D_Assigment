using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Application.targetFrameRate = 144;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), Mathf.RoundToInt(1/Time.deltaTime).ToString());
    }
}
