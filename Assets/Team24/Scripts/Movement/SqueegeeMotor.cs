using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class SqueegeeMotor : MicrogameInputEvents
    {
        Vector2 direction;
        Vector2 desiredPos;
        [SerializeField] float maxLength;
        [SerializeField] GameObject squeegeeObj;
        [SerializeField] float lerpSpeed;
        [SerializeField] float rotationSpeed = 360f;
        public float extensionSpeed = 5f;

        //WiperPlayerMotor wiper;

        //public bool deactivateMovement;

        float targetAngle;
        float targetLength;

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
            //wiper = GetComponent<WiperPlayerMotor>();
        }

        void Update()
        {
            direction = stick.normalized;

            // -= because rotation is backwards
            targetAngle -= direction.x * rotationSpeed * Time.deltaTime;
            // Make sure we don't go negative or too far
            targetLength = Mathf.Clamp(targetLength + direction.y * extensionSpeed * Time.deltaTime, 0, maxLength);

            // Huzzah target quaternion (-90 to convert from math space to unity space)
            Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle - 90f);

            // Calculate the X and Y of the direction, converting the angle to radians
            float x = Mathf.Cos(targetAngle * Mathf.Deg2Rad);
            float y = Mathf.Sin(targetAngle * Mathf.Deg2Rad);
            // Offset from our position to the target
            Vector3 offset = new Vector3(x, y) * targetLength;

            Vector3 targetPos = transform.position + offset;
            targetPos.z = squeegeeObj.transform.position.z; // Keep it in line with the squeegee

            squeegeeObj.transform.position = Vector3.Lerp(squeegeeObj.transform.position, targetPos, lerpSpeed * Time.deltaTime);
            squeegeeObj.transform.rotation = Quaternion.Slerp(squeegeeObj.transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
           

            /*
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

            if (deactivateMovement)
            {
                wiper.enabled = false;
                if (button1.IsPressed())
                {
                    desiredPos = currentPos + direction * maxLength;
                }
                else
                {
                    desiredPos = currentPos + direction;
                }
            }
            else
            {
                wiper.enabled = true;
                desiredPos = currentPos + direction * maxLength;
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
            */
        }
    }
}