using UnityEngine;
using System.Collections;
using System.Linq;

public class GameCamera : MonoBehaviour
{
    public GameObject target;       //Public variable to store a reference to the player game object
    public Vector3 startingOffset;         //Private variable to store the offset distance between the player and camera
    public float distanceFromCenter;
    public float leashLength = 2f;           //Threshold the player must move out for the camera to move
    public bool isPullingLeash;
    public float damping = 1;
    public Bounds boundaries;

    //private Vector3 centerFrom(Transform[] elements)
    //{
    //    Vector3[] positions = elements.Select(item => item.position).ToArray<Vector3>;
    //    return new Vector3(0, 0, 0);
    //} 

    // Use this for initialization
    void Start()
    {
        boundaries = new Bounds();
        boundaries.center = transform.position;
        boundaries.Expand(leashLength*2);
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        transform.LookAt(target.transform);
        startingOffset = transform.position - target.transform.position; //Need to center the camera view in the target, instead of only storing the offset.
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boundaries.center, boundaries.size);
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        distanceFromCenter = Vector3.Distance(boundaries.center, target.transform.position);
        if (Mathf.Abs(distanceFromCenter) >= leashLength) isPullingLeash = true;
        if (Mathf.Abs(distanceFromCenter) < leashLength) isPullingLeash = false;

        //bool isIntersecting = boundaries.Intersects(target.GetComponent<Collider>().bounds);
        if (isPullingLeash)
        {
            //// Put the center behind the character leashing
            //if (boundaries.center.x < target.transform.position.x) boundaries.center = target.transform.position - new Vector3(leashLength, 0, 0);
            //if (boundaries.center.x > target.transform.position.x) boundaries.center = target.transform.position + new Vector3(leashLength, 0, 0);
            //if (boundaries.center.z < target.transform.position.z) boundaries.center = target.transform.position - new Vector3(0, 0, leashLength);
            //if (boundaries.center.z > target.transform.position.z) boundaries.center = target.transform.position + new Vector3(0, 0, leashLength);

            Vector3 tail = target.transform.forward * -1 * leashLength; // only works (and badly at that) for one target
            boundaries.center = target.transform.position + tail;

            // The camera now follows the center
            Vector3 desiredPosition = boundaries.center + startingOffset;
            Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.position = position;
        }
    }
}
