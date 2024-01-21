using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Door : MonoBehaviour, IInteractible
{
    [Header("Settings")]
    [SerializeField] private GameObject door;
    [SerializeField] private float speed;
    [SerializeField] private float angle;
    [Header("Special")]
    [SerializeField] private bool doubleDoor;
    [SerializeField] private GameObject secondDoor;
    private bool forward;
    private bool locked;
    private bool opening;
    private Quaternion rotation;
    private Quaternion baseRot = Quaternion.Euler(0, 0, 0);
    private bool open;
    private float hitBoxSize;
    private float hitBoxCenter;
    
    public void Interact()
    {
        if (!locked)
        {
            SetForward();
            SetRotation();
        }
    }
    private void Awake()
    {
        hitBoxSize = GetComponent<BoxCollider>().size.z;
        hitBoxCenter = GetComponent<BoxCollider>().center.z;
    }

    private void Update()
    {
        OpenDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemie" && !opening)
        {
            if (Vector3.Dot(transform.forward, other.transform.localPosition - transform.localPosition) < 0)
            {
                rotation = Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y - angle, transform.localRotation.z));
                opening = true;
            }
            else
            {
                rotation = Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y + angle, transform.localRotation.z));
                opening = true;
            }
        }
    }

    private void SetForward()
    {
        if (!open)
        {
            if (Vector3.Dot(transform.forward, CharacterController.instance.gameObject.transform.localPosition - transform.localPosition) < 0)
            {
                forward = false;
            }
            else
            {
                forward = true;
            }
        }
    }
    private void SetRotation()
    {
        if (open)
        {
            rotation = baseRot;
            opening = true;
        }
        else
        {
            if (forward)
            {
                rotation = Quaternion.Euler(new Vector3(door.transform.localRotation.x, door.transform.localRotation.y - angle, door.transform.localRotation.z));
            }
            else
            {
                rotation = Quaternion.Euler(new Vector3(door.transform.localRotation.x, door.transform.localRotation.y + angle, door.transform.localRotation.z));
            }
            opening = true;
        }
    }
    private void OpenDoor()
    {
        if (opening)
        {
            door.transform.localRotation = Quaternion.Slerp(door.transform.localRotation, rotation, speed * Time.deltaTime);
            if(doubleDoor)
            {
                secondDoor.transform.localRotation = Quaternion.Slerp(secondDoor.transform.localRotation,Quaternion.Inverse(rotation), speed * Time.deltaTime);
            }
            if (Mathf.Abs(door.transform.localRotation.y - rotation.y) < 0.02f)
            {
                opening = false; 
                open = !open;
                if (open)
                {
                    GetComponent<BoxCollider>().size += new Vector3(0, 0, hitBoxSize);
                    if(forward)
                    {

                        GetComponent<BoxCollider>().center += new Vector3(0, 0, hitBoxCenter * 2);
                    }
                    else
                    {

                        GetComponent<BoxCollider>().center -= new Vector3(0, 0, hitBoxCenter * 2);
                    }
                }
                else
                {
                    GetComponent<BoxCollider>().size -= new Vector3(0, 0, hitBoxSize);
                    if (forward)
                    {

                        GetComponent<BoxCollider>().center -= new Vector3(0, 0, hitBoxCenter * 2);
                    }
                    else
                    {

                        GetComponent<BoxCollider>().center += new Vector3(0, 0, hitBoxCenter * 2);
                    }
                }
            }
        }
    }
}
