using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    public class SqueegeeMotor : MicrogameInputEvents
    {
        Vector2 direction;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            direction = stick.normalized;
        }
    }
}