using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int range;
    [SerializeField] private GameObject shotOrigin;
    [SerializeField] private int maxAmmo;

    [SerializeField] private bool recoil;
    [SerializeField] private float amountRecoil;

    private int ammo;

    private void Start()
    {
        ammo = maxAmmo;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        if (ammo > 0)
        {
            if (Physics.Raycast(shotOrigin.transform.position, shotOrigin.transform.forward, out hit, range))
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<LifeManager>().TakeDamage(5);
                }
                Recoil();
                ammo--;
                Debug.DrawLine(shotOrigin.transform.position, hit.point, Color.red, 10f);
            }
        }
    }

    private void Reload()
    {
        ammo = maxAmmo;
    }

    private void Recoil()
    {
        if(recoil)
        {
            transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z - amountRecoil);
            transform.localRotation = Quaternion.Euler(-amountRecoil*100,0,0);
        }
    }
}
