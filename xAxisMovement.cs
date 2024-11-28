using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xAxisMovement : MonoBehaviour
{
    //rotatation modification on object
    private Quaternion rotationY;
    private float rotationmodify = 0.1f;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch current_position_touch = Input.GetTouch(0);
            if (current_position_touch.phase == TouchPhase.Moved)
            {
                rotationY = Quaternion.Euler(0F, -current_position_touch.deltaPosition.x * rotationmodify, 0f);
                transform.rotation = rotationY * transform.rotation;
            }
        }
    }
}
