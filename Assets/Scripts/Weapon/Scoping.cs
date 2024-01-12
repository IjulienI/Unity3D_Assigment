using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoping : MonoBehaviour
{
    [SerializeField] private Vector3 scopePos;
    private Vector3 basePos;
    private bool isScoping;

    private void Start()
    {
        basePos = transform.localPosition;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (CharacterController.instance.GetCanScope())
            {
                if(CharacterController.instance.state == CharacterController.State.crouch)
                {
                    CharacterController.instance.state = CharacterController.State.crouch;
                }
                else
                {
                    CharacterController.instance.state = CharacterController.State.walk;
                }
                Scope();
                
                CharacterController.instance.action = CharacterController.Action.scope;
            }
            else
            {
                CharacterController.instance.action = CharacterController.Action.nothing;
            }
        }
        else
        {
            UnScope();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) && CharacterController.instance.action == CharacterController.Action.scope)
        {
            CharacterController.instance.action = CharacterController.Action.nothing;
        }
    }

    private void Scope()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, scopePos, 28 * Time.deltaTime);
    }

    private void UnScope()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, basePos, 8.5f * Time.deltaTime);
    }
}