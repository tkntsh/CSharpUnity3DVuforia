using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationForARSelection : MonoBehaviour
{
    public Vector3 speed;

    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, Space.Self);
    }
}
