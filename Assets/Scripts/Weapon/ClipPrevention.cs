using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipPrevention : MonoBehaviour
{
    [SerializeField] private GameObject clipProjector;
    [SerializeField] private float checkDistance;
    [SerializeField] private Vector3 newDirection;

    private float lerpPos;
    private RaycastHit hit;

    private void Update()
    {
        if (Physics.Raycast(clipProjector.transform.position, clipProjector.transform.forward, out hit, checkDistance))
        {
            lerpPos = 1 - (hit.distance / checkDistance);
            GetComponent<Scoping>().SetCanScope(false);
        }
        else
        {
            lerpPos = 0;
            GetComponent<Scoping>().SetCanScope(true);
        }

        Mathf.Clamp01(lerpPos);

        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Vector3.zero),Quaternion.Euler(newDirection), lerpPos);
    }
}
