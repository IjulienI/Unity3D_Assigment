using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSecurity : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.detectable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.instance.detectable = false;
    }
}
