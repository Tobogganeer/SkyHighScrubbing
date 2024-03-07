using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace team24
{
    public class ScaffoldMotor : MicrogameInputEvents
    {
        // This is old, unused code
        /*
        [SerializeField] float buttonSpeed = 10f;
        [SerializeField] float joystickSpeed = 3f;
        [SerializeField] float acceleration = 5f;

        [Space]
        [SerializeField] Vector2 xBounds;
        [SerializeField] Vector2 yBounds;


        Vector2 direction;
        Vector2 velocity;
        Vector2 targetVelocity;

        public static bool UsingButtons { get; private set; }

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
        *

        void Update()
        {
            direction = stick.normalized;

            // If we aren't moving the joystick
            /*
            if (stick == Vector2.zero)
            {
                // Set our target to zero for now
                targetVelocity = Vector2.zero;

                // Check if we are trying to go up or down quickly
                if (button1.IsPressed())
                    targetVelocity = Vector3.up * buttonSpeed;
                //transform.Translate(Vector3.up * buttonSpeed * Time.deltaTime);
                if (button2.IsPressed())
                    targetVelocity = Vector3.down * buttonSpeed;
                //transform.Translate(Vector3.down * buttonSpeed * Time.deltaTime);

                // Tell the squeegee if it should stop
                UsingButtons = button1.IsPressed() || button2.IsPressed();
            }
            else
            {
            *
            targetVelocity = direction * joystickSpeed;
            //transform.Translate(direction * joystickSpeed * Time.deltaTime);
            // The squeegee can go
            UsingButtons = false;
            //}

            velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * acceleration);
            transform.Translate(velocity * Time.deltaTime);

            // Make sure we don't go out of bounds
            float clampedX = Mathf.Clamp(transform.position.x, xBounds.x, xBounds.y);
            float clampedY = Mathf.Clamp(transform.position.y, yBounds.x, yBounds.y);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }

        private void OnDrawGizmosSelected()
        {
            // Draw the limits of where we can go
            Gizmos.color = Color.yellow;
            Vector3 min = new Vector3(xBounds.x, yBounds.x, -1f);
            Vector3 max = new Vector3(xBounds.y, yBounds.y, 1f);
            Vector3 size = max - min;
            Gizmos.DrawWireCube(min + size / 2f, size);
        }
        */
    }
}