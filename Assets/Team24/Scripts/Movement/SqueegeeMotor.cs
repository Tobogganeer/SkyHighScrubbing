using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace team24
{
    public class SqueegeeMotor : MicrogameInputEvents
    {
        Vector2 direction;
        Vector2 desiredPos;
        [SerializeField] float maxLength;
        [SerializeField] GameObject squeegeeObj;
        [SerializeField] float lerpSpeed;

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
            if (stick != Vector2.zero)
            {
                direction = stick.normalized;
            }

            //Handle rotation
            Quaternion desiredRot = Quaternion.LookRotation(Vector3.forward, direction);

            if (Vector3.Distance(squeegeeObj.transform.position, transform.position) > 0.1f)
            {
                squeegeeObj.transform.rotation = Quaternion.Lerp(squeegeeObj.transform.rotation, desiredRot, lerpSpeed * Time.deltaTime);
            }
            else
            {
                squeegeeObj.transform.rotation = desiredRot;
            }


            Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
            if (button1.IsPressed())
            {
                desiredPos = currentPos + direction * maxLength;
            }
            else
            {
                desiredPos = currentPos + direction;
            }


            //Handle movement
            Vector2 pos2d;
            if (stick == Vector2.zero) 
            {
                pos2d = Vector2.Lerp(squeegeeObj.transform.position, transform.position, lerpSpeed * Time.deltaTime);
            }
            else
            {
                pos2d = Vector2.Lerp(squeegeeObj.transform.position, desiredPos, lerpSpeed * Time.deltaTime);
            }


            squeegeeObj.transform.position = new Vector3(pos2d.x, pos2d.y, squeegeeObj.transform.position.z);
        }
    }
}