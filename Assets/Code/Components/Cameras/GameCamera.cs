using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameCamera : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();       // Public variable to store a reference to the players
    public Vector3 centerOffset;         // Private variable to store the offset distance between the center and camera

    // Thresholds configuration
    public float smallFovDistance = 4f;
    public float bigFovDistance = 8f;

    // FOV configuration
    public float smallFov;
    public float fovDifference = 8f;
    public float bigFov;

    // Operational variables
    public float distanceFromCenter;
    public bool isPullingLeash;
    public float damping = 1;
    public Camera gameCamera;

    public void addTarget(GameObject gameObject)
    {
        targets.Add(gameObject);
        centerOffset = transform.position - targetsCenter(targets);
    }

    // TODO: make private what should be private 
    // Use this for initialization
    void Start()
    {
        gameCamera = GetComponent<Camera>();
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        //transform.LookAt(target.transform);
        //Offset to the center: or "real" target
        centerOffset = transform.position - targetsCenter(targets); // Need to center the camera view in the target, instead of only storing the offset.

        smallFovDistance = (Screen.height / 2.5f)/Screen.dpi;
        bigFovDistance = 2.5f * smallFovDistance;

        smallFov = gameCamera.fieldOfView;
        bigFov = smallFov + fovDifference;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position - centerOffset;

        Gizmos.color = Color.red; // big area
        Gizmos.DrawWireSphere(center, bigFovDistance);
        Gizmos.DrawLine(center, center + new Vector3(bigFovDistance, 0, 0));

        Gizmos.color = Color.yellow; // small area
        Gizmos.DrawWireSphere(center, smallFovDistance);
        Gizmos.DrawLine(center, center + new Vector3(1, smallFovDistance, 0));

        Gizmos.color = Color.white; // target
        targets.ForEach((element) => Gizmos.DrawLine(center, element.transform.position));
    }

    void setFieldOfView(float fov)
    {
        gameCamera.fieldOfView = Mathf.Lerp(gameCamera.fieldOfView, fov, Time.deltaTime * damping);
    }

    private Vector3 targetsCenter(List<GameObject> targets)
    {
        if (targets.Count == 0) return new Vector3(0, 0, 0);
        if (targets.Count == 1) return targets[0].transform.position;

        List<Vector3> positions = targets.Select(item => item.transform.position).ToList<Vector3>();
        Vector3 sum = Vector3.zero;
        positions.ForEach(pos => sum += pos);
        return sum / positions.Count();
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // TODO:
        // 1. Change FOV when going from small to big -> ok know how to do it
        // 1b. One player wont ever get zoomed out. Poor little thing, right?
        // 2. You cant move outside radius or screen
        // 3. Okay, follow target if you try to go outside radius: 1 player OK, more players is trickier
        // 4. Targets fight for camera dominance... this is many players. At least 2.
        Vector3 center = transform.position - centerOffset; // not center, center of targets calculated earlier :c
        distanceFromCenter =  Vector3.Distance(center, targets[0].transform.position);
        //if (distanceFromCenter < smallFovDistance) setFieldOfView(smallFov);
        //if (smallFovDistance < distanceFromCenter && distanceFromCenter < bigFovDistance) setFieldOfView(bigFov);

        float leashLength = bigFovDistance;
        if (targets.Count == 1) leashLength = smallFovDistance;
        if (distanceFromCenter >= leashLength) isPullingLeash = true;
        if (distanceFromCenter < leashLength) isPullingLeash = false;
        
        if (isPullingLeash)
        {
            Vector3 distanceOffset = targets[0].transform.position - center;
            Vector3 desiredPosition = transform.position + distanceOffset;
            Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.position = position;
        }
    }
}
