using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Timeline.AnimationPlayableAsset;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject weapon;
    [SerializeField] private RawImage crossair;
    [SerializeField] private LayerMask ground;
    private bool isGrounded;
    [SerializeField] private float playerHeight, walkSpeed, runSpeed, crouchSpeed, camSpeed, baseFov, runFov, crouchFov;
    private State state;
    private Action action;
    private float speed, baseHeight, crouchHeight;
    private Vector2 input;
    private Vector2 mouseInput;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private RayDetection rayDetection;
    private Vector3 weaponBasePos;
    private Vector3 weaponScopePos = new Vector3(0,-0.099f,0);


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rayDetection = GetComponent<RayDetection>();
        cam.fieldOfView = baseFov;
        capsuleCollider = GetComponent<CapsuleCollider>();
        baseHeight = capsuleCollider.height;
        weaponBasePos = weapon.transform.localPosition;
    }

    private void Update()
    {

        SpeedControl();
        //Cases
        switch (state)
        {
            case State.idle:
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFov, 5f * Time.deltaTime);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 6 * Time.deltaTime);
                break;
            case State.walk:
                if(action == Action.scope)
                {
                    speed = crouchSpeed;
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFov, 5f * Time.deltaTime);
                }
                else
                {
                    speed = walkSpeed;
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFov, 5f * Time.deltaTime);
                }                
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 6 * Time.deltaTime);
                break;
            case State.slowWalk:
                speed = crouchSpeed;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFov, 5f * Time.deltaTime);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 28 * Time.deltaTime);
                break;
            case State.run:
                speed = runSpeed;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, runFov, 5f * Time.deltaTime);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 28 * Time.deltaTime);
                break;
            case State.crouch:
                speed = crouchSpeed;                
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, crouchHeight, 8.5f * Time.deltaTime);
                break;
        }

        switch (action)
        {
            case Action.nothing:
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, weaponBasePos, 8.5f * Time.deltaTime);
                crossair.enabled = true;
                break;
            case Action.scope:
                weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, weaponScopePos, 28 * Time.deltaTime);
                crossair.enabled = false;
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
        if (Input.GetKey(KeyCode.Mouse0))
        {
            rayDetection.Shoot(cam.gameObject);
        }

        //Movements
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Vector3 movementZ = transform.forward * input.x;
        //Vector3 movementX = transform.right * input.y;
        //transform.position += (movementX + movementZ) * speed * Time.deltaTime;

        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        rb.AddForce(moveDirection.normalized * speed * Time.deltaTime * 200, ForceMode.Force);

        if(input.x < .5f && input.y < .5f && input.x > -.5f && input.y > -.5f && state != State.crouch)
        {
            state = State.idle;
        }
        else if (state == State.idle)
        {
            state = State.walk;
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, ground);

        if(isGrounded)
        {
            rb.drag = 5;
        }
        else
        {
            rb.drag = 0;
            speed /= 5;
        }
    }

    private void FixedUpdate()
    {
        mouseInput += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.rotation = Quaternion.Euler(0, mouseInput.x * camSpeed, 0);
        cam.transform.localRotation = Quaternion.Euler(-mouseInput.y * camSpeed, 0, 0);
    }

    private void SpeedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatvel.magnitude > speed)
        {
            Vector3 limitedVel = flatvel.normalized* speed;
            rb.velocity = new Vector3(limitedVel.x,rb.velocity.y, limitedVel.z);
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
