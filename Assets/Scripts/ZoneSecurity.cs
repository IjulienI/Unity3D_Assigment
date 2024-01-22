using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneSecurity : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI secur;
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.detectable = true;
        secur.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.instance.detectable = false;
        secur.gameObject.SetActive(false);
    }
}
