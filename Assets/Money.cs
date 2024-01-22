using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour, IInteractible
{
    [SerializeField] private int moneyAmount;
    public void Interact()
    {
        CharacterController.instance.gameObject.GetComponent<VariableStock>().money += moneyAmount;
        Destroy(gameObject);
    }
}
