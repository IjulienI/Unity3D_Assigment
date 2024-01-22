using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractible
{
    [Header("References")]
    [SerializeField] private GameObject leverArm;

    [Header("Settings")]
    public Type type;
    [SerializeField] private float speed;
    [SerializeField] private List<GameObject> objects = new List<GameObject>();

    private bool open;
    private float rotation = - 180f;
    public void Interact()
    {
        open = !open;
        foreach (GameObject obj in objects)
        {
            if(type == Type.Door)
            {
                obj.GetComponent<Door>().SetLock(!open);
            }
            else if (type == Type.Camera)
            {

            }
        }
    }

    private void Update()
    {
        MoveLever();
    }

    private void MoveLever()
    {
        if (open)
        {
            leverArm.transform.localRotation = Quaternion.Slerp(leverArm.transform.localRotation, Quaternion.Euler(rotation, 0,0), speed * Time.deltaTime);
        }
        else
        {
            leverArm.transform.localRotation = Quaternion.Slerp(leverArm.transform.localRotation, Quaternion.Euler(0, 0, 0), speed * Time.deltaTime);
        }        
    }

    public enum Type
    {
        Door,
        Camera
    }
}
