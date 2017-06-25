using UnityEngine;
using System.Collections;
using System.Linq;

public class GameCamera : MonoBehaviour
{
    public GameObject target;       //Public variable to store a reference to the player game object
    public Vector3 startingOffset;         //Private variable to store the offset distance between the player and camera
    public float smallFovDistance = 4f;
    public float smallFov = 16f;
    public float bigFovDistance = 8f;
    public float bigFov = 20f;
    public float distanceFromCenter;
    public bool isPullingLeash;
    public float damping = 1;
    public Camera gameCamera;

    // TODO: make private what should be private 

    //private Vector3 centerFrom(Transform[] elements)
    //{
    //    Vector3[] positions = elements.Select(item => item.position).ToArray<Vector3>;
    //    return new Vector3(0, 0, 0);
    //} 

    // Use this for initialization
    void Start()
    {
        gameCamera = GetComponent<Camera>();
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        transform.LookAt(target.transform);
        startingOffset = transform.position - target.transform.position; // Need to center the camera view in the target, instead of only storing the offset.

        smallFovDistance = (Screen.height / 2.5f)/Screen.dpi;
        bigFovDistance = 2.5f * smallFovDistance;
        Debug.Log("smallFovDistance: " + smallFovDistance);
        Debug.Log("bigFovDistance: " + bigFovDistance);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position - startingOffset;

        Gizmos.color = Color.red; // big area
        Gizmos.DrawWireSphere(center, bigFovDistance);
        Gizmos.DrawLine(center, center + new Vector3(bigFovDistance, 0, 0));

        Gizmos.color = Color.yellow; // small area
        Gizmos.DrawWireSphere(center, smallFovDistance);
        Gizmos.DrawLine(center, center + new Vector3(1, smallFovDistance, 0));

        Gizmos.color = Color.white; // target
        Gizmos.DrawLine(center, target.transform.position);
    }

    //float calculateFovDistance(float multiplier)
    //{
    //    gameCamera.ViewportToWorldPoint()
    //}

    void setFieldOfView(float fov)
    {
        gameCamera.fieldOfView = Mathf.Lerp(gameCamera.fieldOfView, fov, Time.deltaTime * damping);
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // TODO:
        // 1. change FOV when going from small to big
        // 2. you cant move outside radius or screen
        // 3. okay, follow target if you try to go outside radius
        // 4. Targets fight for camera dominance
        Vector3 center = transform.position - startingOffset;
        distanceFromCenter =  Vector3.Distance(center, target.transform.position);
        if (distanceFromCenter < smallFovDistance) setFieldOfView(smallFov);
        if (smallFovDistance < distanceFromCenter && distanceFromCenter < bigFovDistance) setFieldOfView(bigFov);
        if (distanceFromCenter >= bigFovDistance) isPullingLeash = true;
        if (distanceFromCenter < bigFovDistance) isPullingLeash = false;
           


        /* old code

        //bool isIntersecting = boundaries.Intersects(target.GetComponent<Collider>().bounds);
        if (isPullingLeash)
        {
            //// Put the center behind the character leashing
            //if (boundaries.center.x < target.transform.position.x) boundaries.center = target.transform.position - new Vector3(leashLength, 0, 0);
            //if (boundaries.center.x > target.transform.position.x) boundaries.center = target.transform.position + new Vector3(leashLength, 0, 0);
            //if (boundaries.center.z < target.transform.position.z) boundaries.center = target.transform.position - new Vector3(0, 0, leashLength);
            //if (boundaries.center.z > target.transform.position.z) boundaries.center = target.transform.position + new Vector3(0, 0, leashLength);

            Vector3 tail = target.transform.forward * -1 * bigFovDistance; // only works (and badly at that) for one target
            transform.position = target.transform.position + tail;

            // The camera now follows the center
            Vector3 desiredPosition = transform.position + startingOffset;
            Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.position = position;
        }*/
    }
}
