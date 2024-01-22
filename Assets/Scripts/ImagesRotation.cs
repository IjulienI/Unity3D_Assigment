using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagesRotation : MonoBehaviour
{

    private void Update()
    {
        transform.LookAt(CharacterController.instance.transform.position);
    }
}
