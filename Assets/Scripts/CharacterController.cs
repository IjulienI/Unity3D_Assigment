using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject weapon;
    [SerializeField] private float walkSpeed, runSpeed, crouchSpeed, camSpeed, baseFov, runFov, crouchFov;
    private State state;
    private Action action;
    private float speed, baseHeight, crouchHeight;
    private Vector2 input;
    private Vector2 mouseInput;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Vector3 weaponBasePos;
    private Vector3 weaponScopePos = new Vector3(0,-0.099f,0);


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam.fieldOfView = baseFov;
        capsuleCollider = GetComponent<CapsuleCollider>();
        baseHeight = capsuleCollider.height;
        weaponBasePos = weapon.transform.localPosition;
    }

    private void Update()
    {
        //Cases
        switch (state)
        {
            case State.idle:
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFov, .05f);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 0.1f);
                break;
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
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, crouchHeight, 0.03f);
                break;
        }

        switch (action)
        {
            case Action.nothing:
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, weaponBasePos, 0.5f);
                break;
            case Action.scope:
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, weaponScopePos, 0.5f);
                break;
        }

        //Inputs
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (state != State.crouch && state != State.idle && action != Action.scope)
            {
                state = State.run;
            }
        }
        else
        {
            if (state != State.crouch)
            {
                state = State.walk;
            }
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (state != State.run)
            {
                state = State.crouch;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (state == State.crouch)
            {
                state = State.walk;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            state = State.walk;
            action = Action.scope;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            action = Action.nothing;
        }

        //Movements
        input = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        Vector3 movementZ = transform.forward * input.x * speed * Time.deltaTime;
        Vector3 movementX = transform.right * input.y * speed * Time.deltaTime;
        transform.position += movementX + movementZ;

        mouseInput += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.rotation = Quaternion.Euler(0, mouseInput.x * camSpeed, 0);
        cam.transform.localRotation = Quaternion.Euler(-mouseInput.y * camSpeed, 0, 0);

        if(input.x + input.y == 0 && state != State.crouch)
        {
            state = State.idle;
        }
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
}
