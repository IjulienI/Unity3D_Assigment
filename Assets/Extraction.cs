using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Extraction : MonoBehaviour, IInteractible
{
    public void Interact()
    {
        SceneManager.LoadScene("EXTRACT");
    }
}
