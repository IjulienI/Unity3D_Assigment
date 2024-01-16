using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoping : MonoBehaviour
{
    [SerializeField] private Vector3 scopePos;
    private Vector3 basePos;
    private bool canScope;
    private bool scoping;

    private void Start()
    {
        basePos = transform.localPosition;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (canScope)
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
                scoping = true;
                
                CharacterController.instance.action = CharacterController.Action.scope;
            }
            else
            {
                CharacterController.instance.action = CharacterController.Action.nothing;
                UnScope();
                scoping = false;
            }
        }
        else
        {
            UnScope();
            scoping = false;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) && CharacterController.instance.action == CharacterController.Action.scope)
        {
            CharacterController.instance.action = CharacterController.Action.nothing;
            scoping = false;
        }
    }

    private void Scope()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, scopePos, 28 * Time.deltaTime);
        CharacterController.instance.ChangeInteract(false);
    }

    private void UnScope()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, basePos, 8.5f * Time.deltaTime);
        CharacterController.instance.ChangeInteract(true);
    }

    public void SetCanScope(bool value)
    {
        canScope = value;
    }

    public Vector3 GetScopePos()
    {
        return scopePos;
    }

    public bool IsScoping()
    {
        return scoping;
    }
}