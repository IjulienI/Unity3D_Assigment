using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, IInteractible
{
    public void Interact()
    {
        Debug.Log("Interact");
    }
}
