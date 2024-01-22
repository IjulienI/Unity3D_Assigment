using UnityEngine;

public class Detection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float detectionTime;
    [SerializeField] float speed;

    private float time;
    private bool Triggered;
    private GameObject player;

    private RaycastHit hit;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.position - player.transform.position);
        if (Triggered)
        {
            if (Physics.Raycast(transform.position, transform.position - player.transform.position, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    time += (speed / hit.distance) * Time.deltaTime;
                    if (time > detectionTime)
                    {
                        Debug.Log("Detected");
                    }
                }
            }
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
}
