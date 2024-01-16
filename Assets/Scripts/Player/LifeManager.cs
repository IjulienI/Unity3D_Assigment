using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float life;
    [SerializeField] private float maxLife;

    public void TakeDamage(float damage)
    {
        life -= damage;
        if(life <= 0)
        {
            life = 0;
            //death
            Debug.Log("death");
        }
    }

    public void Heal(float amount)
    { 
        life += amount;
        if(life > maxLife)
        {
            life = maxLife;
        }
    }
}
