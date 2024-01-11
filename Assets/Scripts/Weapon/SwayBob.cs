using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SwayBob : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private Rigidbody rb;

    [Header("Settings")]
    [SerializeField] private bool sway,swayRotation,bobOffset,bobSway;
    private Vector3 originalPos;
    private Vector3 scopePos = new Vector3(0, -0.099f, 0);
    private Vector3 pos;
    private float originalSmooth;
    private float scopeSmooth = .5f;
    private float originalRotSmooth;
    private float scopeRotSmooth = .5f;

    [Header("Sway")]
    [SerializeField] private float step = 0.01f;
    [SerializeField] private float maxStepDistance = 0.06f;
    [SerializeField] private float smooth = 10f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    [SerializeField] private float rotationStep = 4f;
    [SerializeField] private float maxRotationStep = 5f;
    [SerializeField] private float smoothRot = 12f;
    Vector3 swayEulerRot;

    [Header("Bobbing")]
    [SerializeField] private float speedCurve;

    private float curveSin { get => Mathf.Sin(speedCurve); }
    private float curveCos { get => Mathf.Cos(speedCurve); }

    [SerializeField] private Vector3 traveLimit = Vector3.one * 0.025f;
    [SerializeField] private Vector3 bobLimit = Vector3.one * 0.01f;

    private Vector3 bobPosition;

    [Header("Bob Rotation")]
    [SerializeField] private Vector3 multiplier;
    private Vector3 bobEulerRotation;

    private bool scope;

    private void Awake()
    {
        originalPos = transform.localPosition;
        originalSmooth = smooth;
        originalRotSmooth = smoothRot;
    }

    private void Update()
    {
        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        CompositePositionRotation();

        if (!CharacterController.instance.IsScoping() && !scope)
        {
            pos = scopePos;
            smooth = originalSmooth;
            smoothRot = originalRotSmooth;
        }
        else
        {
            pos = originalPos;
            smooth = scopeSmooth;
            smoothRot = scopeRotSmooth;
        }
    }

    private void Sway()
    {
        if (sway == false) { swayPos = Vector3.zero; return; }

        Vector2 invertLook = new Vector2 (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * -step;
        invertLook.x = Mathf.Clamp(invertLook.x + pos.x, - maxStepDistance + pos.x, maxStepDistance + pos.x);
        invertLook.y = Mathf.Clamp(invertLook.y + pos.y, -maxStepDistance + pos.y, maxStepDistance + pos.y);

        swayPos = invertLook;
    }

    private void SwayRotation()
    {
        if (swayRotation== false) { swayEulerRot = Vector3.zero; return; }
        {
            Vector3 invertLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * -step;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

            swayEulerRot = new Vector3(invertLook.y,invertLook.x, invertLook.x);
        }
    }

    private void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);

        transform.localRotation = Quaternion.Slerp(transform.localRotation,Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    private void BobOffset()
    {
        speedCurve += Time.deltaTime * (CharacterController.instance.IsGrounded() ? rb.velocity.magnitude : 1f) + 0.01f;

        if(bobOffset == false) { bobPosition = Vector3.zero; return; }

        bobPosition.x = (curveCos * bobLimit.x * (CharacterController.instance.IsGrounded() ? 1 : 0)) - (Input.GetAxis("Horizontal") * traveLimit.x);

        bobPosition.y = (curveSin * bobLimit.y - (rb.velocity.y * traveLimit.y));

        bobPosition.z = -(Input.GetAxis("Vertical") * traveLimit.z);
    }

    private void BobRotation()
    {
        if (bobSway == false) { bobEulerRotation = Vector3.zero; return; }

        bobEulerRotation.x = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) :
                                                                                                                multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        bobEulerRotation.y = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) != Vector2.zero ? multiplier.z * curveCos * Input.GetAxis("Horizontal") : 0);
    }
}
