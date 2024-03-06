using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace team24
{
    public class ScaffoldMotor : MicrogameInputEvents
    {
        [SerializeField] float buttonSpeed;
        [SerializeField] float joystickSpeed;
        [SerializeField] Vector2 xBounds;
        [SerializeField] Vector2 yBounds;

        Vector2 direction;

        public static bool UsingButtons { get; private set; }

        #region Button overrides
        /*
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
        */
        #endregion

        void Update()
        {
            direction = stick.normalized;
            Vector3 dir = direction;

            // If we aren't moving the joystick
            if (stick == Vector2.zero)
            {
                // Check if we are trying to go up or down quickly
                if (button1.IsPressed())
                    transform.Translate(Vector3.up * buttonSpeed * Time.deltaTime);
                if (button2.IsPressed())
                    transform.Translate(Vector3.down * buttonSpeed * Time.deltaTime);

                // Tell the squeegee if it should stop
                UsingButtons = button1.IsPressed() || button2.IsPressed();
            }
            else
            {
                transform.Translate(dir * joystickSpeed * Time.deltaTime);
                // The squeegee can go
                UsingButtons = false;
            }

            // Make sure we don't go out of bounds
            float clampedX = Mathf.Clamp(transform.position.x, xBounds.x, xBounds.y);
            float clampedY = Mathf.Clamp(transform.position.y, yBounds.x, yBounds.y);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }

        private void OnDrawGizmos()
        {
            // Draw the limits of where we can go
            Gizmos.color = Color.yellow;
            Vector3 min = new Vector3(xBounds.x, yBounds.x, -1f);
            Vector3 max = new Vector3(xBounds.y, yBounds.y, 1f);
            Vector3 size = max - min;
            Gizmos.DrawWireCube(min + size / 2f, size);
        }
    }
}