using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace team24
{
    public class ScaffoldMotor : MicrogameInputEvents
    {
        Vector2 direction;
        [SerializeField] float buttonSpeed;
        [SerializeField] float joystickSpeed;

        #region Button overrides
        protected override void OnButton1Pressed(InputAction.CallbackContext context)
        {

            Debug.Log("Do action 1");

        }

        protected override void OnButton1Released(InputAction.CallbackContext context)
        {

            Debug.Log("Stop action 1");

        }

        protected override void OnButton2Pressed(InputAction.CallbackContext context)
        {

            Debug.Log("Do action 2");

        }

        protected override void OnButton2Released(InputAction.CallbackContext context)
        {

            Debug.Log("Stop action 2");

        }
        #endregion

        void Update()
        {
            direction = stick.normalized;
            Vector3 dir = direction;

            if(button1.IsPressed() && stick == Vector2.zero)
            {
                transform.Translate(Vector3.down * buttonSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(dir * joystickSpeed * Time.deltaTime);
            }
        }
    }
}