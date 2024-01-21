using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField] private float distance;

    public float GetDistance()
    {
        return distance;
    }

}
