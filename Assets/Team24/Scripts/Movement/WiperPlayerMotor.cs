using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace team24
{
    public class WiperPlayerMotor : MicrogameInputEvents
    {
        Rigidbody rb;
        [SerializeField] float moveSpeed;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
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

        void FixedUpdate()
        {
            if(button1.IsPressed())
            {
                rb.velocity = Vector3.left * moveSpeed;
            }

            if(button2.IsPressed())
            {
                rb.velocity = Vector3.right * moveSpeed;
            }

            if(!button1.IsPressed() && !button2.IsPressed())
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
