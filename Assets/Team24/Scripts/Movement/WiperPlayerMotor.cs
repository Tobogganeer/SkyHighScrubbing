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
        [SerializeField] Transform leftBoundary;
        [SerializeField] Transform rightBoundary;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        #region Button Overrides
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

        void FixedUpdate()
        {
            if(button1.IsPressed() && transform.position.x > leftBoundary.position.x)
            {
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            }

            if(button2.IsPressed() && transform.position.x < rightBoundary.position.x)
            {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            }
        }
    }
}
