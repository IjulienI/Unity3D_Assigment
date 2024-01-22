using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cameraObj;

    [Header("Settings")]
    [SerializeField] private float rotation;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;

    private bool back;
    private bool canRot = true;
    private float rot;

    private void Start()
    {
        rot = rotation;
    }

    private void Update()
    {
        if(canRot)
        {
            cameraObj.transform.localRotation = Quaternion.Lerp(cameraObj.transform.localRotation, Quaternion.Euler(0, rotation, 0), speed * Time.deltaTime);
        }

        if (back)
        {
            if (Mathf.Abs(cameraObj.transform.localRotation.y - Quaternion.Euler(0, cameraObj.transform.localRotation.y + rotation, 0).y) < 0.02f)
            {
                canRot = false;
                back = !back;
                rotation = rot;
                Invoke(nameof(Remove), waitTime);
            }
        }
        else
        {
            if(Mathf.Abs(cameraObj.transform.localRotation.y - Quaternion.Euler(0, cameraObj.transform.localRotation.y + rotation, 0).y) < 0.02f)
            {
                canRot = false;
                back = !back;
                rotation = 0;
                Invoke(nameof(Remove), waitTime);
            }
        }
    }

    private void Remove()
    {
        canRot = true;
    }
}
