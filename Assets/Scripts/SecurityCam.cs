using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCam : MonoBehaviour, IInteractible
{
    private bool activate = true;
    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Aad");
        }
    }
}
