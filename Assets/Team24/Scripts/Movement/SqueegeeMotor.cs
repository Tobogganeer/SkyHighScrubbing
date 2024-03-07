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
        [SerializeField] float extensionSpeed;
        [SerializeField] float lerpSpeed;
        [SerializeField] float rotationSpeed;

        WiperPlayerMotor wiper;

        public bool deactivateMovement;
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
            wiper = GetComponent<WiperPlayerMotor>();
        }

        void Update()
        {
            direction = stick.normalized;

                /*//Handle rotation
                /*Quaternion desiredRot = Quaternion.LookRotation(Vector3.forward, direction);

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
                    pos2d = Vector2.Lerp(squeegeeObj.transform.position, transform.position, lerpSpeed * Time.deltaTime);
                }

                squeegeeObj.transform.position = new Vector3(pos2d.x, pos2d.y, squeegeeObj.transform.position.z);
                */

            //Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
            //Vector2 pos2d = new Vector2(squeegeeObj.transform.position.x, squeegeeObj.transform.position.y);

            //desiredPos = currentPos + new Vector2(squeegeeObj.transform.up.x, squeegeeObj.transform.up.y) * maxLength;

            if (direction != Vector2.zero)
            {
                if (direction.y > 0)
                {
                    if (Vector3.Distance(squeegeeObj.transform.position, transform.position) < maxLength)
                    {
                        squeegeeObj.transform.Translate(direction * extensionSpeed * Time.deltaTime, Space.Self);
                    }
                }
                else if (direction.y < 0)
                {
                    squeegeeObj.transform.position = Vector2.Lerp(squeegeeObj.transform.position, new Vector2(transform.position.x, transform.position.y), extensionSpeed * Time.deltaTime);
                }
                if (direction.x < 0)
                {
                    squeegeeObj.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
                }
                else if (direction.x > 0)
                {
                    squeegeeObj.transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
                }
            }

            //squeegeeObj.transform.position = new Vector3(pos2d.x, pos2d.y, squeegeeObj.transform.position.z);
        }
    }
}