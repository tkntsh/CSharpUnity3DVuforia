using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scalingGameObject : MonoBehaviour
{
    //zooming speed of gameobject
    public float zoomSpeed = 0.1f;
    //scale limits
    public float minScale = 0.5f;
    public float maxScale = 3.0f;
    //checking touch and distance made
    private Vector2 touchStartPos0;
    private Vector2 touchStartPos1;
    private float startDistance;

    void Update()
    {
        //check for two fingers on screen
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            //checks if the touch has started and saves initial position
            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                touchStartPos0 = touch0.position;
                touchStartPos1 = touch1.position;
                startDistance = Vector2.Distance(touchStartPos0, touchStartPos1);
            }
            //checking if the touches have been moved on the screen
            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                //saving distance between the first position and the current position
                Vector2 currentTouchPos0 = touch0.position;
                Vector2 currentTouchPos1 = touch1.position;
                float currentDistance = Vector2.Distance(currentTouchPos0, currentTouchPos1);
                //calculating distance between the first position and the current position
                float distanceDelta = currentDistance - startDistance;
                float scaleFactor = 1 + distanceDelta * zoomSpeed * Time.deltaTime;

                //scaling the gameobject according to current distance and start distance
                Vector3 newScale = transform.localScale * scaleFactor;
                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);
                transform.localScale = newScale;

                //update start distance for next frame
                startDistance = currentDistance;
            }
        }
    }
}
