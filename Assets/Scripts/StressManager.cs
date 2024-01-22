using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    [SerializeField] private float stress;
    [SerializeField] private float maxStress;
    [SerializeField] private float minStress;

    [SerializeField] private float timeForDecrease;
    [SerializeField] private float speed;
    [SerializeField] private Slider stressbar;

    private float timer;

    private void Update()
    {
        if(timer > timeForDecrease && stress > minStress)
        {
            stress = Mathf.Lerp(stress, minStress, speed * Time.deltaTime);
        }
        else
        {
            timer += 1 * Time.deltaTime;
        }

        stressbar.value = stress / maxStress;
    }


    public void AddStress(float amount)
    {
        stress += amount;
        if(stress >= maxStress)
        {
            SceneManager.LoadScene("GAMEOVER");
        }
        timer = 0;
    }
}
