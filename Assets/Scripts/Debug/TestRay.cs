using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRay : MonoBehaviour
{
    private RayDetection rayDetection;
    void Start()
    {
        rayDetection = GetComponent<RayDetection>();
        shoot();
    }

    private void shoot()
    {
        rayDetection.Shoot(gameObject);
        Invoke(nameof(shoot), 1f);
    }
}
