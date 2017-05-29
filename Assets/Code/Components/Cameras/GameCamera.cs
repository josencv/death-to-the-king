using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{
    public GameObject target;       //Public variable to store a reference to the player game object
    public Vector3 startingOffset;         //Private variable to store the offset distance between the player and camera
    public Vector3 distanceFromCenter;
    public float leash = 2f;           //Threshold the player must move out for the camera to move
    public bool isPullingLeash;


    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        transform.LookAt(target.transform);
        startingOffset = transform.position - target.transform.position; //Need to center the camera view in the target, instead of only storing the offset.
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        distanceFromCenter = transform.position - target.transform.position;
        if (Mathf.Abs(distanceFromCenter.x) >= leash) isPullingLeash = true;
        if (Mathf.Abs(distanceFromCenter.x) < leash) isPullingLeash = false;
        if (isPullingLeash)
        {
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = target.transform.position + startingOffset;
        }
    }
}
