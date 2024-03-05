using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueegeeMask : MonoBehaviour
{
    // This class makes a trail of gameobjects as this object moves
    // This is to prevent gaps in the squeegee due to moving too far
    // in between frames.

    public int lerpCopies = 20;
    public GameObject squeegeePrefab;

    List<Transform> copies = new List<Transform>();
    Vector3 lastPosition;
    Quaternion lastRotation;

    private void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        for (int i = 0; i < lerpCopies; i++)
            copies.Add(Instantiate(squeegeePrefab, transform).transform);
    }

    private void LateUpdate()
    {
        PositionCopies();
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void PositionCopies()
    {
        Vector3 start = lastPosition;
        Vector3 end = transform.position;

        Quaternion startRot = lastRotation;
        Quaternion endRot = transform.rotation;

        for (int i = 0; i < copies.Count; i++)
        {
            float fac = (float)i / copies.Count;
            copies[i].position = Vector3.Lerp(start, end, fac);
            copies[i].rotation = Quaternion.Slerp(startRot, endRot, fac);
        }
    }
}
