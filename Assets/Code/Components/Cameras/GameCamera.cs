using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{
    public GameObject target;       //Public variable to store a reference to the player game object
    public Vector3 startingOffset;         //Private variable to store the offset distance between the player and camera
    public float distanceFromCenter;
    public float leashLength = 2f;           //Threshold the player must move out for the camera to move
    public bool isPullingLeash;
    public Vector3 center;
    public float damping = 1;

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        transform.LookAt(target.transform);
        center = transform.position;
        startingOffset = transform.position - target.transform.position; //Need to center the camera view in the target, instead of only storing the offset.
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        distanceFromCenter = Vector3.Distance(center, target.transform.position);
        if (Mathf.Abs(distanceFromCenter) >= leashLength) isPullingLeash = true;
        if (Mathf.Abs(distanceFromCenter) < leashLength) isPullingLeash = false;
        if (isPullingLeash)
        {
            // Put the center behind the character leashing
            if (center.x < target.transform.position.x) center = target.transform.position - new Vector3(leashLength, 0, 0);
            if (center.x > target.transform.position.x) center = target.transform.position + new Vector3(leashLength, 0, 0);
            if (center.z < target.transform.position.z) center = target.transform.position - new Vector3(0, 0, leashLength);
            if (center.z > target.transform.position.z) center = target.transform.position + new Vector3(0, 0, leashLength);

            // The camera now follows the center
            Vector3 desiredPosition = center + startingOffset;
            Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.position = position;
        }
    }
}
