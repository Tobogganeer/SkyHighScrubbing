using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class SqueegeeMotor : MicrogameInputEvents
    {
        [SerializeField] ControlMode controlMode;
        [SerializeField] GameObject squeegeeObj;
        [SerializeField] float maxLength;

        [Header("Point Towards Joystick")]
        [SerializeField] float lerpSpeed = 5f;
        [Space]
        [SerializeField] float minLength = 0.5f;
        [SerializeField] float sweepSpeed = 2f;

        [Header("In, Out, Rotate")]
        [SerializeField] float moveLerpSpeed = 8f;
        [SerializeField] float rotateSlerpSpeed = 4f;
        [SerializeField] float rotationSpeed = 360f;
        [SerializeField] float extensionSpeed = 5f;

        [Header("Jet Style")]
        [SerializeField] float jetMoveSpeed = 10f;

        [Header("Squeegee Head Tilt")]
        public bool buttonsControlSqueegeeHeadTilt = true;
        public Transform squeegeeHead;
        public float headTiltSpeed = 90f;
        public float headTiltAngle = 45f;

        float headTilt;

        Vector2 direction;
        Vector2 desiredPos;

        Vector3 lastJetPosition;

        float targetAngle = 90;
        float targetLength;

        float sweepTimer;


        void Update()
        {
            direction = stick.normalized;

            // Choose which controls we are using
            if (controlMode == ControlMode.PointTowardsJoystick || controlMode == ControlMode.PointTowardsJoystick_Sweep)
                PointTowardsJoystick();
            else if (controlMode == ControlMode.InOutRotate)
                InOutRotate();
            else
                JetStyle();

            TiltHead();
        }

        void PointTowardsJoystick()
        {
            bool movementIsDesired = stick != Vector2.zero;

            if (movementIsDesired)
                direction = stick.normalized;

            //Handle rotation
            Quaternion desiredRot = Quaternion.LookRotation(Vector3.forward, direction);

            // If the squeegee is held out rotate it smoothly
            if (Vector3.Distance(squeegeeObj.transform.position, transform.position) > 0.1f)
                squeegeeObj.transform.rotation = Quaternion.Slerp(squeegeeObj.transform.rotation, desiredRot, lerpSpeed * Time.deltaTime);
            // Otherwise it is close to our body - snap the rotation
            else
                squeegeeObj.transform.rotation = desiredRot;


            float length = maxLength;

            // Sweep if we wanna sweep
            if (controlMode == ControlMode.PointTowardsJoystick_Sweep)
            {
                if (movementIsDesired)
                {
                    sweepTimer += Time.deltaTime * sweepSpeed;
                    length = Mathf.PingPong(sweepTimer, maxLength - minLength) + minLength;
                }
                else
                {
                    sweepTimer = 0;
                }
            }

            Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
            desiredPos = currentPos + direction * length;

            //Handle movement
            Vector2 pos2d;
            // If we are trying to move the squeegee, move it towards the desired position
            if (movementIsDesired)
                pos2d = Vector2.Lerp(squeegeeObj.transform.position, desiredPos, lerpSpeed * Time.deltaTime);
            // Otherwise bring it back home
            else
                pos2d = Vector2.Lerp(squeegeeObj.transform.position, transform.position, lerpSpeed * Time.deltaTime);


            squeegeeObj.transform.position = new Vector3(pos2d.x, pos2d.y, squeegeeObj.transform.position.z);
        }

        void InOutRotate()
        {
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

            squeegeeObj.transform.position = Vector3.Lerp(squeegeeObj.transform.position, targetPos, moveLerpSpeed * Time.deltaTime);
            squeegeeObj.transform.rotation = Quaternion.Slerp(squeegeeObj.transform.rotation, targetRot, rotateSlerpSpeed * Time.deltaTime);
        }

        void JetStyle()
        {
            Vector3 desiredPosition = transform.position;
            if (stick != Vector2.zero)
                desiredPosition += (Vector3)stick.normalized * maxLength;

            desiredPosition.z = squeegeeObj.transform.position.z;

            Vector3 desiredDirection = (desiredPosition - lastJetPosition).normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, desiredDirection);

            squeegeeObj.transform.position = Vector3.Lerp(squeegeeObj.transform.position, desiredPosition, jetMoveSpeed * Time.deltaTime);
            squeegeeObj.transform.rotation = Quaternion.Slerp(squeegeeObj.transform.rotation, desiredRotation, jetMoveSpeed * Time.deltaTime);

            lastJetPosition = squeegeeObj.transform.position;
        }

        void TiltHead()
        {
            // Calculate current tilt
            if (buttonsControlSqueegeeHeadTilt)
            {
                int dir = 0;
                if (button1.IsPressed()) dir--;
                if (button2.IsPressed()) dir++;
                headTilt = Mathf.MoveTowards(headTilt, dir * headTiltAngle, Time.deltaTime * headTiltSpeed);
            }
            else
                headTilt = 0f;

            squeegeeHead.transform.localRotation = Quaternion.Euler(0, 0, headTilt);
        }

        public enum ControlMode
        {
            PointTowardsJoystick_Sweep,
            PointTowardsJoystick,
            InOutRotate,
            JetStyle
        }
    }
}