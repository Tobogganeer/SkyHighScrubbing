using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace team24
{
    public class PhysicsScaffoldMotor : MicrogameInputEvents
    {
        [SerializeField] float maxSpeed = 3f;
        [SerializeField] float acceleration = 5f;
        [SerializeField] float friction = 0.1f;
        [SerializeField] float windForce = 0.5f;

        bool right;
        Vector2 direction;

        Rigidbody rb;

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

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            direction = stick.normalized;

            //Acceleration
            rb.AddForce(direction * acceleration, ForceMode.VelocityChange);

            //Friction force
            rb.velocity -= friction * rb.velocity;

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

            Sway();
        }

        void Sway()
        {
            float time = (float)Time.timeAsDouble;
            float x = (Mathf.Cos(time) * 2f * windForce);
            float y = (Mathf.Sin(time * 2f) * windForce);
            rb.AddForce(new Vector3(x, y, 0));
        }
    }
}