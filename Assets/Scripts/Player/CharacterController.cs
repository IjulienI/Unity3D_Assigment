using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public static CharacterController instance;
    [Header("External")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject weapon;
    [SerializeField] private RawImage crossair;
    [SerializeField] private LayerMask ground;
    [Header("")]

    [Header("Settings")]

    [Header("Movement/Sensibility")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float camSpeed;
    [SerializeField] private float stairsMultiplier;

    [Header("FOV")]
    [SerializeField] private float runFov;
    [SerializeField] private float baseFov;
    [SerializeField] private float crouchFov;
    [Header("")]

    [Header("DontTouch!")]
    public State state;
    public Action action;
    [SerializeField] private float crouchHeight;

    //Privates bool
    private bool isGrounded;
    private bool onStairs;

    //Private floats
    private float speed, baseHeight,stairsRunSpeed,stairsWalkSpeed,stairsCrouchSpeed,playerHeight;

    //Private Vectors
    private Vector2 input;
    private Vector2 mouseInput;
    //private Vector3 weaponBasePos;
    //private Vector3 weaponScopePos = new Vector3(0,-0.099f,0);

    //Private External
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;


    private void Awake()
    {
        instance = this;
        //weaponBasePos = weapon.transform.localPosition;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        baseHeight = capsuleCollider.height;
        cam.fieldOfView = baseFov;
        stairsWalkSpeed = walkSpeed * stairsMultiplier;
        stairsRunSpeed = runSpeed * stairsMultiplier -.5f;
        stairsCrouchSpeed = crouchSpeed * stairsMultiplier +.5f;
    }

    private void Update()
    {
        SpeedControl();
        InputUpdate();
        StatesUpdate();
        MovementUpdate();
        GroundedUpdate();
    }

    private void FixedUpdate()
    {
        RotationFixedUpdate();
    }

    //For update
    private void InputUpdate()
    {
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
    }

    private void StatesUpdate()
    {
        switch (state)
        {
            case State.idle:
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFov, 5f * Time.deltaTime);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 6 * Time.deltaTime);
                playerHeight = baseHeight;
                break;
            case State.walk:
                if (action == Action.scope)
                {
                    if (onStairs)
                    {
                        speed = stairsCrouchSpeed;
                    }
                    else
                    {
                        speed = crouchSpeed;
                    }
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFov, 5f * Time.deltaTime);
                }
                else
                {
                    if (onStairs)
                    {
                        speed = stairsWalkSpeed;
                    }
                    else
                    {
                        speed = walkSpeed;
                    }
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFov, 5f * Time.deltaTime);
                }
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 6 * Time.deltaTime);
                playerHeight = baseHeight;
                break;
            case State.slowWalk:
                if (onStairs)
                {
                    speed = stairsCrouchSpeed;
                }
                else
                {
                    speed = crouchSpeed;
                }
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFov, 5f * Time.deltaTime);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 28 * Time.deltaTime);
                playerHeight = baseHeight;
                break;
            case State.run:
                if (onStairs)
                {
                    speed = stairsRunSpeed;
                }
                else
                {
                    speed = runSpeed;
                }
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, runFov, 5f * Time.deltaTime);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, baseHeight, 28 * Time.deltaTime);
                playerHeight = baseHeight;
                break;
            case State.crouch:
                if(onStairs)
                {
                    speed = stairsCrouchSpeed;
                }
                else
                {
                    speed = crouchSpeed;
                }
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, crouchHeight, 8.5f * Time.deltaTime);
                playerHeight = crouchHeight;
                break;
        }

        switch (action)
        {
            case Action.nothing:
                crossair.enabled = true;
                break;
            case Action.scope:
                crossair.enabled = false;
                break;
        }
    }

    private void MovementUpdate()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        rb.AddForce(moveDirection.normalized * speed * Time.deltaTime * 200, ForceMode.Force);

        if (input.x < .5f && input.y < .5f && input.x > -.5f && input.y > -.5f && state != State.crouch)
        {
            state = State.idle;
        }
        else if (state == State.idle)
        {
            state = State.walk;
        }
    }

    private void RotationFixedUpdate()
    {
        mouseInput += new Vector2(Input.GetAxis("Mouse X") * camSpeed * Time.fixedDeltaTime, Input.GetAxis("Mouse Y") * camSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, mouseInput.x, 0);

        mouseInput.y = Mathf.Clamp(mouseInput.y, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(-mouseInput.y, 0, 0);
    }

    private void GroundedUpdate()
    {
        RaycastHit hit;

        //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, ground);
        
        if(Physics.Raycast(transform.position, Vector3.down,out hit, playerHeight * 0.5f + 0.1f, ground))
        {
            isGrounded = true;
            if(hit.collider.tag == "Stairs")
            {
                onStairs = true;
            }
            else
            {
                onStairs = false;
            }
        }
        else
        {
            isGrounded = false;
            onStairs = false;
        }

        if (isGrounded)
        {
            rb.drag = 5;
        }
        else
        {
            rb.drag = 0;
            speed /= 5;
        }
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

    //Enums
    public enum State
    {
        idle,
        walk,
        slowWalk,
        run,
        crouch
    }

    public enum Action
    {
        nothing,
        scope
    }

    //For other scripts

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
