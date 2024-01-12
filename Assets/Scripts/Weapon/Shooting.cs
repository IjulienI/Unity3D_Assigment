using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private int range;
    [SerializeField] private GameObject shotOrigin;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        if(Physics.Raycast(shotOrigin.transform.position,shotOrigin.transform.forward,out hit, range))
        {
            Debug.DrawLine(shotOrigin.transform.position, hit.point, Color.red, 10f);
            if(hit.collider.tag == "Player" || hit.collider.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<LifeManager>().TakeDamage(5);
            }
        }
    }
}
