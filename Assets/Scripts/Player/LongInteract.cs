using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongInteract : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private Camera cam;

    private RaycastHit hit;
    private void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance))
        {
            Debug.DrawLine(cam.transform.position, hit.point, Color.green, 10f);
            if (hit.collider.gameObject.TryGetComponent(out IInteractible obj))
            {
                obj.Interact();
            }
        }
    }
}
