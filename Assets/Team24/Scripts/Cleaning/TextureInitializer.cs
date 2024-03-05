using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace team24
{
    [RequireComponent(typeof(Camera))]
    public class TextureInitializer : MonoBehaviour
    {
        void Start()
        {
            Camera cam = GetComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;

            cam.Render(); // Clear it to black

            cam.clearFlags = CameraClearFlags.Nothing;
        }
    }
}
