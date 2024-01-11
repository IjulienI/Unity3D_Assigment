using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoping : MonoBehaviour
{
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
                
                CharacterController.instance.action = CharacterController.Action.scope;
            }
            else
            {
                CharacterController.instance.action = CharacterController.Action.nothing;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            CharacterController.instance.action = CharacterController.Action.nothing;
        }
    }
}