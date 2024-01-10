using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDetection : MonoBehaviour
{
    [SerializeField] private static int shootDistance = 1000;
    [SerializeField] private static int interactDistance = 10;

    public void Shoot(GameObject origin)
    {
        RaycastHit hit;
        if(Physics.Raycast(origin.transform.position, origin.transform.forward, out hit, shootDistance))
        {
            Debug.DrawRay(origin.transform.position, origin.transform.forward, Color.red, 2f);
        }
    }
}
