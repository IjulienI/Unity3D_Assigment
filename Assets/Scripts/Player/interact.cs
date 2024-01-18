using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

interface IInteractible
{
    public void Interact();
}

public class interact : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Image indicator;
    private float distance = 1000;

    private RaycastHit hit;

    private void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance))
        {
            if(hit.collider.gameObject.TryGetComponent(out IInteractible obj))
            {
                if (hit.collider.gameObject.GetComponent<Interactible>().GetDistance() >= hit.distance)
                {
                    if (Input.GetKeyDown(KeyCode.E) && CharacterController.instance.CanInteract())
                    {
                        obj.Interact();
                    }
                    else
                    {
                        IndicatorState(false);
                    }
                    Debug.DrawLine(cam.transform.position, hit.point, Color.green, .1f);
                    IndicatorState(true);
                }
                else
                {
                    IndicatorState(false);
                }
            }
            else
            {
                IndicatorState(false);
            }
        }
        else
        {
            IndicatorState(false);
        }
    }

    private void IndicatorState(bool state)
    {
        indicator.enabled = state;
    }
}
