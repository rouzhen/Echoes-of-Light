using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Player's Transform
    public Transform endLimit; // GameObject that indicates end of map
    private float offsetX; // initial x-offset between camera and player
    private float offsetY; // initial y-offset between camera and player
    private float startX; // smallest x-coordinate of the Camera
    private float endX; // largest x-coordinate of the camera
    private float viewportHalfWidth;

    void Start()
    {
        // get coordinate of the bottomleft of the viewport
        // z doesn't matter since the camera is orthographic
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);

        // Store both X and Y offsets
        offsetX = this.transform.position.x - player.position.x;
        offsetY = this.transform.position.y - player.position.y;

        startX = this.transform.position.x;
        endX = endLimit.transform.position.x - viewportHalfWidth;
    }

    void Update()
    {
        float desiredX = player.position.x + offsetX;
        float desiredY = player.position.y + offsetY; // Follow Y position

        // check if desiredX is within startX and endX
        if (desiredX > startX && desiredX < endX)
            this.transform.position = new Vector3(desiredX, desiredY, this.transform.position.z);
        else
            // Still update Y even when X is clamped
            this.transform.position = new Vector3(this.transform.position.x, desiredY, this.transform.position.z);
    }
}
