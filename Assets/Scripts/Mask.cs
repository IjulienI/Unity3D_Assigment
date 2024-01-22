using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject hand;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            GameManager.instance.maskOn = true;
            weapon.SetActive(true);
            CharacterController.instance.gameObject.GetComponent<ChangeItems>().AddItem(weapon);
            CharacterController.instance.gameObject.GetComponent<ChangeItems>().RemoveItem(hand);
            CharacterController.instance.gameObject.GetComponent<ChangeItems>().RemoveItem(gameObject);
        }
    }
}
