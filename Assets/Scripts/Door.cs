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
    private Quaternion baseRot;
    private bool open;
    
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
        baseRot = transform.rotation;
    }

    private void Update()
    {
        OpenDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemie" && !opening)
        {
            if (Vector3.Dot(transform.forward, other.transform.position - transform.position) < 0)
            {
                rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y - angle, transform.rotation.z));
                opening = true;
            }
            else
            {
                rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y + angle, transform.rotation.z));
                opening = true;
            }
        }
    }

    private void SetForward()
    {
        if (Vector3.Dot(transform.forward, CharacterController.instance.gameObject.transform.position - transform.position) < 0)
        {
            forward = false;
        }
        else
        {
            forward = true;
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
                rotation = Quaternion.Euler(new Vector3(door.transform.rotation.x, door.transform.rotation.y - angle, door.transform.rotation.z));
            }
            else
            {
                rotation = Quaternion.Euler(new Vector3(door.transform.rotation.x, door.transform.rotation.y + angle, door.transform.rotation.z));
            }
            opening = true;
        }
    }
    private void OpenDoor()
    {
        if (opening)
        {
            door.transform.rotation = Quaternion.Slerp(door.transform.rotation, rotation, speed * Time.deltaTime);
            if(doubleDoor)
            {
                secondDoor.transform.rotation = Quaternion.Slerp(secondDoor.transform.rotation,Quaternion.Inverse(rotation), speed * Time.deltaTime);
            }
            if (Mathf.Abs(door.transform.rotation.y - rotation.y) < 0.02f)
            {
                opening = false;
                open = !open;
            }
        }
    }
}
