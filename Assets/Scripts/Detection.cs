using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Detection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float detectionTime;
    [SerializeField] float speed;
    [SerializeField] GameObject warning;

    private float time;
    private bool Triggered;
    private GameObject player;
    private Color baseColor;
    private bool activate = true;

    private RaycastHit hit;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        baseColor = warning.GetComponent<RawImage>().color;
    }

    private void Update()
    {        
        if (Triggered && GameManager.instance.detectable && activate)
        {
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit))
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position);
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Player")
                {
                    time += (speed / hit.distance) * Time.deltaTime;
                    CharacterController.instance.gameObject.GetComponent<StressManager>().AddStress(.2f);
                    if (time > detectionTime)
                    {
                        SceneManager.LoadScene("GAMEOVER");
                    }
                }
            }
        }
        else
        {
            time -= (speed / hit.distance) * Time.deltaTime;
            if(time < 0)
            {
                time = 0;
            }
        }

        if(time > 0)
        {
            warning.SetActive(true);
            warning.GetComponent<RawImage>().color = Color.Lerp(baseColor, Color.red, time);
        }
        else
        {
            warning.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Triggered = false;
        }
    }

    public void SetActivate(bool state)
    {
        activate = state;
    }
}
