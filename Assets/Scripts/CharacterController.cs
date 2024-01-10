using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float walkSpeed, runSpeed,crouchSpeed, camSpeed, baseFov, runFov, crouchFov;
    private State state;
    private Action action;
    private float speed, baseHeight,crouchHeight;
    private Vector2 input;
    private Vector2 mouseInput;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private bool isSprinting, isCrouching, isScoping;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam.fieldOfView = baseFov;
        capsuleCollider = GetComponent<CapsuleCollider>();
        baseHeight = capsuleCollider.height;
    }

    private void Update()
    {
        //Cases
        switch (state)
        {
            case State.walk:
                speed = walkSpeed;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFov, .05f);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 0.1f);
                break;
            case State.slowWalk:
                speed = crouchSpeed;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFov, .05f);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 0.1f);
                break;
            case State.run:
                speed = runSpeed;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, runFov, .05f);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 0.1f);
                break;
            case State.crouch:
                speed = crouchSpeed;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFov, .05f);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, crouchHeight, 0.003f);
                break;
        }

        //Inputs
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(state != State.crouch && action != Action.scope)
            {
                state = State.run;
            }
        }
        else
        {
            if(state != State.crouch)
            {
                state = State.walk;
            }
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if(state != State.run)
            {
                state = State.crouch;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if(state == State.crouch)
            {
                state = State.walk;
            }
        }

        //Movements
        input = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        Vector3 movementZ = transform.forward * input.x * speed * Time.deltaTime;
        Vector3 movementX = transform.right * input.y * speed * Time.deltaTime;
        transform.position += movementX + movementZ;

        mouseInput += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.rotation = Quaternion.Euler(0, mouseInput.x * camSpeed,0);
        cam.transform.localRotation = Quaternion.Euler(-mouseInput.y * camSpeed, 0, 0) ;
    }

    private enum State
    {
        idle,
        walk,
        slowWalk,
        run,
        crouch
    }
    
    private enum Action
    {
        nothing,
        scope
    }

    private void ResetBool()
    {
        isSprinting = false;
        isCrouching = false ;
        isScoping = false ;
    }
}
