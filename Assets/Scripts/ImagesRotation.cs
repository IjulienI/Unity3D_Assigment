using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagesRotation : MonoBehaviour
{
    [SerializeField] private RawImage image;

    private void Update()
    {
        transform.LookAt(CharacterController.instance.transform.position);
    }
}
